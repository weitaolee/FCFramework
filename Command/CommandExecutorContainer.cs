using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using FC.Framework.Utilities;

namespace FC.Framework
{
    /// <summary>
    /// 命令执行器container
    /// </summary>
    public class CommandExecutorContainer : ICommandExecutorContainer
    {
        private object _writeLock = new object();
        private Dictionary<Type, Type> _executorForCommand = new Dictionary<Type, Type>();


        public Type FindExecutorType<TCommand>() where TCommand : ICommand
        {
            var executorType = default(Type);

            this._executorForCommand.TryGetValue(typeof(TCommand), out executorType);

            return executorType;
        }

        public void RegisterExecutor(Type executorType)
        {
            Check.Argument.IsNotNull(executorType, "executorType");

            if (!executorType.IsClass || executorType.IsAbstract || executorType.IsGenericType || executorType.IsInterface) return;

            var executorTypes = TypeUtil.GetGenericArgumentTypes(executorType, typeof(ICommandExecutor<>));

            lock (this._writeLock)
            {
                foreach (var execType in executorTypes)
                {
                    this._executorForCommand[execType] = executorType;
                }
            }
        }

        public void RegisterExecutors(Assembly assemblyToScan)
        {
            Check.Argument.IsNotNull(assemblyToScan, "assemblyToScan");

            foreach (Type type in assemblyToScan.GetTypes())
            {
                RegisterExecutor(type);
            }
        }

        public void RegisterExecutors(IEnumerable<Assembly> assembliesToScan)
        {
            Check.Argument.IsNotNull(assembliesToScan, "assembliesToScan");

            foreach (var assemblyToScan in assembliesToScan)
            {
                if (!assemblyToScan.FullName.Contains("System."))
                    foreach (Type type in assemblyToScan.GetTypes())
                    {
                        RegisterExecutor(type);
                    }
            }

        }
    }
}
