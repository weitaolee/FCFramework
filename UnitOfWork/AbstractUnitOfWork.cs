using System;
namespace FC.Framework
{
    public abstract class AbstractUnitOfWork : DisposableResource, IUnitOfWork
    {
        private bool _disposed;

        public virtual void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot commit a disposed unit of work.");
            OnCommited();
        }


        protected virtual void OnCommited()
        {
            EventBus.GetUncommitedActions().ForEach(action =>
            {
                action();
            });
        }


        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    EventBus.ClearUncommitedActions();
                }

                _disposed = true;
            }
        }
    }
}
