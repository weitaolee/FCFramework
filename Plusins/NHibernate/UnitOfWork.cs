using System;

namespace FC.Framework.NHibernate
{
    public class UnitOfWork : AbstractUnitOfWork
    {
        private IDatabaseFactory _dbFactory;
        private bool _isDisposed = false;


        public UnitOfWork(IDatabaseFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public override void Commit()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            this._dbFactory.Session.Transaction.Commit();

            base.Commit();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dbFactory.ResetInstance();
                }
                base.Dispose(disposing);
                _isDisposed = true;
            }
        }
    }
}
