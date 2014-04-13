namespace FC.Framework
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 事件分发策略
    /// </summary>
    public enum EventDispatchStrategy
    {
        OnEventRaised = 0,
        OnCommitted = 1
    }
}
