namespace FC.Framework
{
    using System;

    /// <summary>event execute strategy
    ///<remarks>event handler which ues this attribute will be executed after the transaction commits</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AwaitCommittedAttribute : Attribute
    {
        public AwaitCommittedAttribute()
        {
            this.Strategy = EventDispatchStrategy.OnCommitted;
        }

        public EventDispatchStrategy Strategy { get; private set; }
    }
}
