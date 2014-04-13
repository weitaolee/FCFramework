using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

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
