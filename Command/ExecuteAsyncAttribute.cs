using System;

namespace FC.Framework
{
    /// <summary>
    /// Command Execute Attribute,if use this attr, the command will executed aync
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExecuteAsyncAttribute : Attribute
    {
        public ExecuteAsyncAttribute() { }

    }
}
