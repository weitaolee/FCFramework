namespace FC.Framework
{
    using System;
    using System.Diagnostics;
    using FC.Framework.Utilities;

    /// <summary>
    /// 执行策略Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OnEventRaisedAttribute : Attribute
    {
        public OnEventRaisedAttribute()
        {
            this.Strategy = EventDispatchStrategy.OnEventRaised;
        }

        public EventDispatchStrategy Strategy { get; private set; }
    }
}
