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
        private string[] _supportCultures;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strEnumDescription">枚举描述</param>
        /// <param name="supportCultures">支持的语言文化（为空时支持所有语言）</param>
        public EnumDescriptionAttribute(string strEnumDescription, params string[] supportCultures)
        {
            _strDescription = strEnumDescription;
            _supportCultures = supportCultures;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get { return _strDescription; } }
        /// <summary>
        /// 支持的文化
        /// </summary>
        public string[] SupportCultures { get { return _supportCultures; } }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumObj, string culture = "")
        {
            string des = string.Empty;
            System.Reflection.FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            if (fieldInfo != null)
            {
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                if (attribArray.Length > 0)
                {
                    var attr = attribArray[0] as EnumDescriptionAttribute;
                    if (attr.SupportCultures == null || attr.SupportCultures.Count() == 0 || string.IsNullOrEmpty(culture) || attr.SupportCultures.Contains(culture))
                        des = attr.Description;
                }
            }

            return des;
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static bool TryGetEnumDescription(Enum enumObj, string culture, out string description)
        {
            description = string.Empty;
            System.Reflection.FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            if (fieldInfo != null)
            {
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                if (attribArray.Length > 0)
                {
                    var attr = attribArray[0] as EnumDescriptionAttribute;
                    if (attr.SupportCultures == null || attr.SupportCultures.Count() == 0 || string.IsNullOrEmpty(culture) || attr.SupportCultures.Contains(culture))
                        description = attr.Description;
                }
            }

            return string.IsNullOrEmpty(description);
        }
    }
}
