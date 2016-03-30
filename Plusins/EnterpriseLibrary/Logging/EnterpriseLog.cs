namespace FC.Framework.EnterpriseLibrary
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using System.Diagnostics;
    using FC.Framework.Utilities;
    /// <summary>
    /// 基于Microsoft EnterpriseLibrary的Log实现
    /// </summary>
    public class EnterpriseLog : ILog
    {

        private readonly int _frameToSkip;
        public EnterpriseLog(int frameToSkip)
        {
            Check.Argument.IsNotNegativeOrZero(frameToSkip, "frameToSkip");

            _frameToSkip = frameToSkip;
        }

        public void Info(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Logger.Write(BuildEntry(message, TraceEventType.Information));
        }

        public void Debug(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Logger.Write(BuildEntry(message, TraceEventType.Verbose));
        }

        public void Warn(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Logger.Write(BuildEntry(message, TraceEventType.Warning));
        }

        public void Warn(string message, Exception exception)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Logger.Write(BuildEntry(message + ":" + exception.Message, TraceEventType.Warning));
        }

        public void Error(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Logger.Write(BuildEntry(message, TraceEventType.Error));
        }

        public void Error(string message, Exception exception)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Logger.Write(BuildEntry(message + ":" + exception.Message, TraceEventType.Error)); throw new NotImplementedException();
        }


        public void Fatal(string message)
        {
            Logger.Write(BuildEntry(message, TraceEventType.Critical));
        }

        public void Fatal(string message, Exception exception)
        {
            Logger.Write(BuildEntry(message + ":" + exception.Message, TraceEventType.Critical));
        }

        private LogEntry BuildEntry(string message, TraceEventType traceEventType)
        {
            return new LogEntry(message, traceEventType.ToString(), 1, 0, traceEventType, null, null);
        }
    }
}
