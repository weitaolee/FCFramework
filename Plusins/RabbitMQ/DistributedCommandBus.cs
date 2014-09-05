using System;
using RabbitMQ.Client;
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
        private static Func<string, IModel> _getMQChannelFunc;
        private static Func<ICommand, string> _matchExchangeFunc;
        private object _connectLock = new object();

        #region ctor
        public DistributedCommandBus(ICommandExecutorContainer executorContainer, Func<string, IModel> getMQChannelFunc, Func<ICommand, string> matchExchangeFunc)
            : base(executorContainer)
        {
            Check.Argument.IsNotNull(getMQChannelFunc, "getMQChannelFunc");
            Check.Argument.IsNotNull(matchExchangeFunc, "matchExchangeFunc");

            _getMQChannelFunc = getMQChannelFunc;
            _matchExchangeFunc = matchExchangeFunc;
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
            var channelName = "distributedCommandBus";
            var _channel = _getMQChannelFunc(channelName);

            try
            {
                var exchangeName = _matchExchangeFunc(cmd);
                var build = new BytesMessageBuilder(_channel);
                var body = Encoding.UTF8.GetBytes(IoC.Resolve<IJsonSerializer>().Serialize(cmd));
                build.WriteBytes(body);
                ((IBasicProperties)build.GetContentHeader()).DeliveryMode = 2;
                _channel.BasicPublish(exchangeName, "", ((IBasicProperties)build.GetContentHeader()), build.GetContentBody());

                Log.Debug("rabbitmq send cmd " + cmd.ToString() + " to exchange:" + exchangeName);
            }
            catch (IoCException iocex)
            {
                Log.Error("RabbitMQ send message error,because the ioc error:" + iocex.Message);
            }
            catch (System.IO.EndOfStreamException ex)
            {
                if (ex.Message.Equals("SharedQueue closed",StringComparison.OrdinalIgnoreCase))
                try
                {
                    _channel = _getMQChannelFunc(channelName);
                    SendDistributed(cmd);
                }
                catch (Exception eex)
                {
                    Log.Fatal("消息队列服务器连接重试{0}次后仍无法连接成功，可能消息队列服务器已经挂掉，请尽快处理！", eex);
                }

            }
            catch (Exception ex)
            { 
                Log.Info("MQ reconnect-->rabbitmq send message error:" + ex.Message);
            }
        }
    }
}
