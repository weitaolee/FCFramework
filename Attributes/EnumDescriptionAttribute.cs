using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    /// <summary>
    /// 枚举描述Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumDescriptionAttribute : Attribute
    {
        private string _strDescription;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strEnumDescription">枚举描述</param>
        public EnumDescriptionAttribute(string strEnumDescription)
        {
            _strDescription = strEnumDescription;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get { return _strDescription; } }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumObj)
        {
            System.Reflection.FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            if (fieldInfo != null)
            {
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                if (attribArray.Length == 0)
                    return String.Empty;
                else
                {
                    EnumDescriptionAttribute attrib = attribArray[0] as EnumDescriptionAttribute;

                    return attrib.Description;
                }
            }
            else return string.Empty;
        }
    }
}
