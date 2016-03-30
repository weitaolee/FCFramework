using System;

namespace FC.Framework.EntityFramework
{
    public class DatabaseFactory : IDatabaseFactory
    {
        [ThreadStatic]
        private static IDatabase _database;
        //每线程一个，无必要加锁
        //private static ThreadLocal<object> _locker = new ThreadLocal<object>(() => new object());

        public IDatabase DatabaseInstance
        {
            get
            {
                if (_database == null)
                {

                    _database = IoC.Resolve<IDatabase>();
                }

                return _database;
            }
        }


        public void ResetInstance()
        {
            _database.Dispose();
            //dispose后还要设置为null,是因线程池时，线程级变量无法清理
            //手动设置null，解决引用，以便GC回收资源
            _database = null;
        }
    }
}
