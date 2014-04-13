using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FC.Framework
{
    public static class QueryableExtension
    {

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">列表</param>
        /// <param name="propertyName">排序字段名称</param>
        /// <param name="order">排序方式:ASC Or DESC</param>
        /// <returns>排序后的列表</returns>
        [DebuggerStepThrough]
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string order)
            where T : class
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null) throw new ArgumentException("propertyName", "Not Exist");

            ParameterExpression param = Expression.Parameter(type, "p");

            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);

            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

            string methodName = order.ToLower().Equals("desc") ? "OrderByDescending" : "OrderBy";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                                                             new Type[] { type, property.PropertyType },
                                                                           source.Expression,
                                                                           Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
