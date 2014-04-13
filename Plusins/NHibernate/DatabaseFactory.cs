using System;
using FC.Framework;
using System.Collections.Generic;
using NHibernate;

namespace FC.Framework.NHibernate
{
    public class DatabaseFactory : IDatabaseFactory
    {
        [ThreadStatic]
        private static ISession _session;
        //每线程一个，无必要加锁
        //private static ThreadLocal<object> _locker = new ThreadLocal<object>(() => new object());

        public ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = SessionManager.OpenSession();
                    _session.BeginTransaction();
                }

                return _session;
            }
        }


        public void ResetInstance()
        {
            if (_session != null)
                _session.Close();
            //dispose后还要设置为null,是因线程池时，线程级变量无法清理
            //手动设置null，解决引用，以便GC回收资源
            _session = null;
        }
    }
}
