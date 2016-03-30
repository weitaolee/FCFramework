using System;

namespace FC.Framework
{
    /// <summary>
    /// 命令
    /// </summary>
    public class Command : ICommand
    {
        public Command()
        {
            this.ID = Guid.NewGuid().Shrink();
        }
        public string ID { get; private set; }
    }
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        string ID { get; }
    }
}
