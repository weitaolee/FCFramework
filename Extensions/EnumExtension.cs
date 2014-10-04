using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    public static class EnumExtension
    {
        /// <summary>
        ///  获得枚举的描述属性值
        /// </summary>
        /// <param name="sourceEnum"></param>
        /// <returns>description</returns>
        public static string GetDescription(this Enum sourceEnum)
        {
            return EnumDescriptionAttribute.GetEnumDescription(sourceEnum);
        }
    }
}
