using System.ComponentModel;
using System.Reflection;

namespace MorningStar.Common
{
    /// <summary>
    /// 公共帮助类
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 获取类描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetClassDescription(Type type)
        {
            var description = type.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return description?.Description ?? string.Empty;
        }
    }
}
