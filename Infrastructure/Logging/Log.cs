
namespace FC.Framework
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using FC.Framework.Utilities;

    public class Log
    {
        private static ILog InnerLogger
        {
            get
            {
                return IoC.Resolve<ILog>();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Info(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");
            if (InnerLogger != null)
                InnerLogger.Info(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Info(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            if (InnerLogger != null)
                InnerLogger.Info(Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Warning(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            if (InnerLogger != null)
                InnerLogger.Warning(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Warning(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            if (InnerLogger != null)
                InnerLogger.Warning(Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Debug(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            if (InnerLogger != null)
                InnerLogger.Debug(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Debug(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            if (InnerLogger != null)
                InnerLogger.Debug(Format(format, args));
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Error(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            if (InnerLogger != null)
                InnerLogger.Error(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Error(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            if (InnerLogger != null)
                InnerLogger.Error(Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Exception(Exception exception)
        {
            Check.Argument.IsNotNull(exception, "exception");

            if (InnerLogger != null)
                InnerLogger.Exception(exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string Format(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            return format.FormatWith(args);
        }
    }
}
