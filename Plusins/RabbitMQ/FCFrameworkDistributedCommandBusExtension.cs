using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FC.Framework.Utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;

namespace FC.Framework.RabbitMQ
{
    public static class FCFrameworkDistributedCommandBusExtension
    {
        public static FCFramework UseRabbitMQCommandBus(this FCFramework fcframework, Assembly[] assemblies, Func<string, IModel> getMQChannelFunc, Func<ICommand, string> matchExchangeFunc)
        {
            Check.Argument.IsNotNull(assemblies, "assemblies");

            var commandExecutorContainer = new CommandExecutorContainer();

            commandExecutorContainer.RegisterExecutors(assemblies);

            //IoC.Register<IExchangeSettings>(exchangeSettings);
            IoC.Register<IDomainContext, DefaultDomainContext>();

            IoC.Register<ICommandBus>(new DistributedCommandBus(commandExecutorContainer, getMQChannelFunc, matchExchangeFunc));
            return fcframework;
        }
    }
}
