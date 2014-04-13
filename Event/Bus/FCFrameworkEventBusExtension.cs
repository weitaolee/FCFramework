using System.Reflection;
using FC.Framework.Utilities;

namespace FC.Framework
{
    public static class FCFrameworkEventBusExtension
    {
        public static FCFramework UseDefaultEventBus(this FCFramework framework, params Assembly[] assemblies)
        {
            Check.Argument.IsNotNull(assemblies, "assemblies");

            var eventHandlerContainer = new EventHandlerContainer();

            eventHandlerContainer.RegisterHandlers(assemblies);

            IoC.Register<IEventBus>(new DefaultEventBus(eventHandlerContainer));

            return framework;
        }
    }
}
