namespace FC.Framework
{
    using System;

    /// <summary> 
    /// event handler is component,will register into ioc singleton
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        public ComponentAttribute()
        {
            this.Strategy = EventDispatchStrategy.OnCommitted;
        }

        public EventDispatchStrategy Strategy { get; private set; }
    }
}
