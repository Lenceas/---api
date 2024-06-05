using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string MD5Encrytp(string str)
        {
            using MD5 md5 = MD5.Create();
            byte[] newBuffer = MD5.HashData(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new();
            for (int i = 0; i < newBuffer.Length; i++)
            {
                // 大写X：ToString("X2")即转化为大写的16进制。
                // 小写x：ToString("x2")即转化为小写的16进制。
                // 2表示每次输出两位，不足2位的前面补0,如 0x0A 如果没有2,就只会输出0xA
                sb.Append(newBuffer[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}