using System.Reflection;
using FC.Framework.Utilities;
using FC.Framework.Repository;

namespace FC.Framework
{
    public static class FCFrameworkCommandBusExtension
    {
        public static FCFramework UseDefaultCommandBus(this FCFramework framework, params Assembly[] assemblies)
        {
            Check.Argument.IsNotNull(assemblies, "assemblies");

            var commandExecutorContainer = new CommandExecutorContainer();

            commandExecutorContainer.RegisterExecutors(assemblies);

            IoC.Register<IDomainContext, DefaultDomainContext>();

            IoC.Register<ICommandBus>(new DefaultCommandBus(commandExecutorContainer));

            return framework;
        }
    }
}
