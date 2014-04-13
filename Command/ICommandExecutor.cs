using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    /// <summary>
    /// 命令执行器
    /// </summary>
    public interface ICommandExecutor<TCommand> where TCommand : ICommand
    {
        void Execute(TCommand cmd);
    }
}
