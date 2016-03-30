using System;

namespace FC.Framework
{
    [Serializable]
    public class EventHandlerExecption : FCFrameworkException
    {
        public EventHandlerExecption(string message) : base(message) { }

        public EventHandlerExecption(string message, Exception innerException) : base(message, innerException) { }
    }
}
