using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    [Serializable]
    public class CommandExecutionException : Exception
    {
        public CommandExecutionException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public CommandExecutionException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        public int ErrorCode { get; private set; }
    }
}
