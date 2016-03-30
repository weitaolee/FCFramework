using System;

namespace FC.Framework
{
    public abstract class AbstractCommandExecutor<TCommand> : ICommandExecutor<TCommand> where TCommand : ICommand
    {
        protected IDomainContext DomainContext { get; set; }

        public AbstractCommandExecutor()
        {
            this.DomainContext = IoC.Resolve<IDomainContext>();
        }

        public virtual void OnExecuting(TCommand cmd)
        {
            if (this.BeforeExecute != null)
                this.BeforeExecute(cmd);
        }

        public abstract void Execute(TCommand cmd);


        public virtual void OnExecuted(TCommand cmd)
        {
            if (this.AfterExecute != null)
                this.AfterExecute(cmd);
        }

        public event Action<TCommand> BeforeExecute;

        public event Action<TCommand> AfterExecute;
    }
}
