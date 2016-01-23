using System;

namespace FC.Framework
{
    /// <summary>
    /// Command Execute Attribute,if use this attr, the command will executed async
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExecuteAsyncAttribute : Attribute
    {
        public ExecuteAsyncAttribute() { }

    }

    /// <summary>
    /// Command Execute Attribute,if use this attr, the command will executed sync
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExecuteSyncAttribute : Attribute
    {
        public ExecuteSyncAttribute() { }

    } 
}
