namespace FC.Framework.EntityFramework
{
    public interface IDatabaseFactory
    {
        /// <summary>
        /// 获取Database实例
        /// </summary>
        IDatabase DatabaseInstance { get; }

        /// <summary>
        /// 重置Database实例
        /// <remarks>如果事务未提交，重置实例可防止实例复用，导致之前无需提交的更新错误提交</remarks>  
        /// </summary>
        void ResetInstance();
    }
}
