namespace FC.Framework
{
    using System.Diagnostics;
    using System;

    public interface IUnitOfWork : IDisposable
    {
        [DebuggerStepThrough]
        void Commit();
    }
}
