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
        public static string GetDescription(this Enum sourceEnum, string cultrue = "ALL")
        {
            return EnumDescriptionAttribute.GetEnumDescription(sourceEnum, cultrue);
        }

        /// <summary>
        ///  获得枚举的描述属性值
        /// </summary>
        /// <param name="sourceEnum"></param>
        /// <param name="cultrue">语言文化</param>
        /// <returns>if has description</returns>
        public static bool TryGetDescription(this Enum sourceEnum, string cultrue, out string description)
        {
            return EnumDescriptionAttribute.TryGetEnumDescription(sourceEnum, cultrue, out description);
        }

    }
}
