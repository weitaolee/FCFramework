namespace FC.Framework
{
    using System.Diagnostics;
    using System;
    using FC.Framework.Domain;
    public interface IUnitOfWork : IDisposable
    {
        [DebuggerStepThrough]
        void Commit();
    }
}
