using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FC.Framework.Utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;

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
        private object _connectLock = new object();

        public DistributedCommandBus(ICommandExecutorContainer executorContainer,
                                     IExchangeSettings settings)
            : base(executorContainer)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotEmpty(settings.RabbitConnectionString, "settings.RabbitConnectionString");

            this._settings = settings;
            this.ConnectionToRabbitMQServer();
        }

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

                var exchangeName = this._settings.MatchExchange(cmd, this._settings.CoinPairExchanges);
                var build = new BytesMessageBuilder(_channel);
                var body = Encoding.UTF8.GetBytes(IoC.Resolve<IJsonSerializer>().Serialize(cmd));
                build.WriteBytes(body);
                ((IBasicProperties)build.GetContentHeader()).DeliveryMode = 2;
                _channel.BasicPublish(exchangeName, "", ((IBasicProperties)build.GetContentHeader()), build.GetContentBody());

                Log.Info("rabbitmq send cmd " + cmd.ToString() + " to exchange:" + exchangeName);
            }
            catch (IoCException iocex)
            {
                Log.Error("RabbitMQ send message error,because the ioc error:" + iocex.Message);
            }
            catch (Exception ex)//如果发生异常重连rabbitMQ集群，寻找新的可用节点
            {
                _connected = false;
                Log.Info("MQ reconnect-->rabbitmq send message error:" + ex.Message);
                try
                {
                    //自动重连，并重试发送命令
                    this.ConnectionToRabbitMQServer();
                    SendDistributed(cmd);
                }
                catch (Exception innerEx)
                {
                    _connected = false;
                    Log.Error("RabbitMQ reconnect error" + innerEx.Message);
                }
            }
        }

        private void ConnectionToRabbitMQServer()
        {
            if (!_connected)
            {
                lock (_connectLock)
                {
                    if (!_connected)
                    {
                        _factory = new ConnectionFactory();
                        _factory.Uri = this._settings.RabbitConnectionString;
                        _connection = _factory.CreateConnection();
                        _channel = _connection.CreateModel();
                        _connected = true;
                    }
                }
            }
        }
    }
}
