using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FC.Framework.Utilities;
using FC.Framework.Domain;
using System.Threading.Tasks;

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

                var executorType = this._executorContainer.FindExecutorType<TCommand>();
                if (executorType == null)
                    throw new UnknowExecption("Faile to find " + typeof(TCommand).Name + "'s executor type.");

                var executor = this._executorActivator.CreateInstance(executorType) as ICommandExecutor<TCommand>;
                if (executor == null)
                    throw new UnknowExecption("Faile to create instance of " + typeof(TCommand).Name + "'s executor.");

                var needAsync = cmd.GetType().IsDefined(typeof(ExecuteAsyncAttribute), false);

                if (needAsync)
                {
                    Task.Factory.StartNew(() =>
                    {
                        using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                        {
                            executor.Execute(cmd);
                            unitOfWork.Commit();
                        }
                    }).ContinueWith((t) => { Log.Exception(t.Exception); });
                }
                else
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        executor.Execute(cmd);
                        unitOfWork.Commit();
                    }
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
