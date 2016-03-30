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
