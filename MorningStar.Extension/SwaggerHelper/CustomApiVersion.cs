using System.ComponentModel;

namespace MorningStar.Extension
{
    /// <summary>
    /// 自定义Api接口版本
    /// </summary>
    public class CustomApiVersion
    {
        /// <summary>
        /// 自定义Api接口版本枚举
        /// </summary>
        public enum ApiVersions
        {
            /// <summary>
            /// v0
            /// </summary>
            [Description("v0")]
            v0 = 0,

            /// <summary>
            /// v1
            /// </summary>
            [Description("v1")]
            v1 = 1,
        }
    }
}
