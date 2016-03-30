using System;

namespace FC.Framework
{
    [Serializable]
    public class IoCException : FCFrameworkException
    {
        public IoCException(string message) : base(message) { }

        public IoCException(string message, Exception innerException) : base(message, innerException) { }

        public IoCException(Exception innerException) : base("IoC Faile,see inner exception for detail.", innerException) { }
    }
}
