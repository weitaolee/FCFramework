
namespace FC.Framework
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class CollectionExtension
    {
        /// <summary>
        /// 集合是否为空
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="collection">集合</param>
        /// <returns>true or false</returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null) || (collection.Count == 0);
        }
    }
}
