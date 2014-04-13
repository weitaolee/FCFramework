using System;
namespace FC.Framework.Domain
{
    public class DomainException : ApplicationException
    {
        /// <summary>The exception code.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>Parameterized constructor.
        /// </summary>
        /// <param name="code"></param>   
        public DomainException(int code)
            : base()
        {
            Code = code;
        }
        /// <summary>Parameterized constructor.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public DomainException(int code, string message)
            : base(message)
        {
            Code = code;
        }
        /// <summary>Parameterized constructor.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public DomainException(int code, string message, params object[] args)
            : base(string.Format(message, args))
        {
            Code = code;
        }
    }
}
