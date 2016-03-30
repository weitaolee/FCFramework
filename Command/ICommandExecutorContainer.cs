using System;
using System.Collections.Generic;
using System.Reflection;

namespace FC.Framework
{
    public interface ICommandExecutorContainer
    {
        Type FindExecutorType<TCommand>() where TCommand : ICommand;

        void RegisterExecutor(Type executorType);

        void RegisterExecutors(Assembly assemblyToScan);

        void RegisterExecutors(IEnumerable<Assembly> assembliesToScan);

    }
}
