namespace FC.Framework
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand cmd) where TCommand : ICommand;
    }
}
