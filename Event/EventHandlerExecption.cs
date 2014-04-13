using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    [Serializable]
    public class EventHandlerExecption : FCFrameworkException
    {
        public EventHandlerExecption(string message) : base(message) { }

        public EventHandlerExecption(string message, Exception innerException) : base(message, innerException) { }
    }
}
