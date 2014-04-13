namespace FC.Framework
{
    public static class UnitOfWork
    {
        public static IUnitOfWork Begin()
        {
            return IoC.Resolve<IUnitOfWork>();
        }
    }
}
