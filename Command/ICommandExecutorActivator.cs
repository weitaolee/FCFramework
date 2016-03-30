using System;

namespace FC.Framework
{
    public interface ICommandExecutorActivator
    {
        object CreateInstance(Type type);
    }

    public class CommandExecutorActivator : ICommandExecutorActivator
    {
        public object CreateInstance(Type type)
        {
            try
            {
                return IoC.Resolve(type);
            }
            catch (IoCException)
            {
                IoC.Register(type,LifeStyle.Transient);
                return IoC.Resolve(type);
            }
        }
    }
}
