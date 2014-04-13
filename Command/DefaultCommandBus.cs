using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FC.Framework.Utilities;
using FC.Framework.Domain;

namespace FC.Framework
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public class DefaultCommandBus : ICommandBus
    {
        private ICommandExecutorContainer _executorContainer;
        private ICommandExecutorActivator _executorActivator;

        public DefaultCommandBus(ICommandExecutorContainer executorContainer)
        {
            Check.Argument.IsNotNull(executorContainer, "executorContainer");

            this._executorContainer = executorContainer;
            this._executorActivator = new CommandExecutorActivator();
        }

        public virtual void Send<TCommand>(TCommand cmd) where TCommand : ICommand
        {
            Check.Argument.IsNotNull(cmd, "cmd");

            try
            {
                using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                {
                    var executorType = this._executorContainer.FindExecutorType<TCommand>();
                    if (executorType == null)
                        throw new UnknowExecption("Faile to find " + typeof(TCommand).Name + "'s executor type.");

                    var executor = this._executorActivator.CreateInstance(executorType) as ICommandExecutor<TCommand>;
                    if (executor == null)
                        throw new UnknowExecption("Faile to create instance of " + typeof(TCommand).Name + "'s executor.");

                    executor.Execute(cmd);
                    unitOfWork.Commit();
                }
            }
            catch (IoCException)
            {
                throw;
            }
            catch (DomainException domainex)
            {
                throw new CommandExecutionException(domainex.Code, domainex.Message);
            }
            catch (SystemException ex)
            {
                throw new UnknowExecption("Faile to execute " + typeof(TCommand).Name + ",see the inner exception for detail.", ex);
            }
        }
    }
}
