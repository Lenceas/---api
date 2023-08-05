using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace MorningStar
{
    /// <summary>
    /// appsettings.json 操作类
    /// </summary>
    public class AppSettings
    {
        public static IConfiguration? Configuration { get; set; }

        public AppSettings()
        {
            string Path = "appsettings.json";
            if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Development"))
                Path = $"appsettings.Development.json";
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true }).Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string Get(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration?[string.Join(":", sections)] ?? string.Empty;
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> Get<T>(params string[] sections)
        {
            List<T> list = new();
            Configuration?.Bind(string.Join(":", sections), list);
            return list;
        }
    }
}
