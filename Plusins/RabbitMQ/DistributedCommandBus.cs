using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FC.Framework.Utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using RabbitMQ.Client.Exceptions;
using System.Threading;

namespace FC.Framework.RabbitMQ
{
    /// <summary>
    /// 分布式命令总线
    /// </summary>
    public class DistributedCommandBus : DefaultCommandBus
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        [ThreadStatic]
        private static IModel _channel;
        private IExchangeSettings _settings;
        private bool _connected = false;
        private int _retryCount = 0;
        private object _connectLock = new object();

        #region ctor
        public DistributedCommandBus(ICommandExecutorContainer executorContainer,
                                     IExchangeSettings settings)
            : base(executorContainer)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotEmpty(settings.RabbitConnectionString, "settings.RabbitConnectionString");

            this._settings = settings;
            _factory = new ConnectionFactory();
            _factory.Uri = this._settings.RabbitConnectionString;
            this.ConnectionToRabbitMQServer();
        }
        #endregion

        public override void Send<TCommand>(TCommand cmd)
        {
            Check.Argument.IsNotNull(cmd, "cmd");

            var needDistribute = cmd.GetType().IsDefined(typeof(ExecuteDistributedAttribute), false);

            if (needDistribute)
            {
                this.SendDistributed(cmd);
            }
            else
            {
                base.Send(cmd);
                Log.Info("send cmd " + cmd.ToString() + " to default command bus");
            }
        }

        private void SendDistributed<TCommand>(TCommand cmd) where TCommand : ICommand
        {
            try
            {
                if (!_connected)
                    throw new Exception("distributed command bus could not send before connect.");

                if (_channel == null)
                    _channel = this._connection.CreateModel();

                var exchangeName = this._settings.MatchExchangeFunc(cmd, this._settings.Exchanges);
                var build = new BytesMessageBuilder(_channel);
                var body = Encoding.UTF8.GetBytes(IoC.Resolve<IJsonSerializer>().Serialize(cmd));
                build.WriteBytes(body);
                ((IBasicProperties)build.GetContentHeader()).DeliveryMode = 2;
                _channel.BasicPublish(exchangeName, "", ((IBasicProperties)build.GetContentHeader()), build.GetContentBody());

                _retryCount = 0;
                Log.Debug("rabbitmq send cmd " + cmd.ToString() + " to exchange:" + exchangeName);
            }
            catch (IoCException iocex)
            {
                Log.Error("RabbitMQ send message error,because the ioc error:" + iocex.Message);
            }
            catch (AlreadyClosedException ex)
            {
                if (_retryCount < 3)
                {
                    Log.Error("DistributedCommandBus发送消息时，发现消息队列服务器已关闭");
                    Log.Info("{0}秒后，会再次重新连接消息队列服务器".FormatWith(30));
                    Thread.Sleep(30 * 1000);
                    _retryCount += 1;
                    try
                    {
                        //断开已有连接
                        this.DisconnectionToRabbitMQServer();
                        //自动重连，并重试发送命令
                        this.ConnectionToRabbitMQServer();
                        SendDistributed(cmd);
                    }
                    catch { }
                }
                else
                {
                    Log.Fatal("消息队列服务器连接重试{0}次后仍无法连接成功，可能消息队列服务器已经挂掉，请尽快处理！", _retryCount);
                }
            }
            catch (Exception ex)//如果发生异常重连rabbitMQ集群，寻找新的可用节点
            {
                _connected = false;
                Log.Info("MQ reconnect-->rabbitmq send message error:" + ex.Message);
            }
        }

        private void ConnectionToRabbitMQServer()
        {
            Log.Info("重新连接消息队列服务器...");
            if (!_connected)
            {
                lock (_connectLock)
                {
                    if (!_connected)
                    {
                        _connection = _factory.CreateConnection();
                        _channel = _connection.CreateModel();
                        _connected = true;

                        Log.Info("成功连接到消息队列服务器");
                    }
                }
            }
        }

        private void DisconnectionToRabbitMQServer()
        {
            Log.Info("释放消息队列服务器连接...");

            try
            {
                _channel.Close();
                _connection.Close();
                _connected = false;
            }
            catch (Exception ex)
            {
                Log.Warn("释放消息队列服务器连接时出现了错误", ex);
            }

            Log.Info("释放消息队列服务器连接完毕!");
        }
    }
}
