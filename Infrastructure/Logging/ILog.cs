using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        void Info(string message);

        void Debug(string message);

        void Warning(string message);

        void Error(string message);

        void Exception(Exception exception);
    }
}
