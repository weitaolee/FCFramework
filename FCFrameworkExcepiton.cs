using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    public class FCFrameworkException : SystemException
    {
        public FCFrameworkException(string message) : base(message) { }

        public FCFrameworkException(string message, Exception innerException) : base(message, innerException) { }

    }
}
