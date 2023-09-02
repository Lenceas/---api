using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MorningStar
{
    /// <summary>
    /// 全局类，用于提供访问已注册服务的方法
    /// </summary>
    public static class App
    {
        private static IServiceProvider? _serviceProvider;
        private static IHttpContextAccessor? _httpContext;
        private static IWebHostEnvironment? _webHostEnvironment;

        /// <summary>
        /// 初始化全局 App 实例，必须在应用程序启动时调用
        /// </summary>
        /// <param name="serviceProvider">依赖注入容器</param>
        /// <param name="httpContextAccessor">上下文对象</param>
        public static void Initialize(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _httpContext = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        /// <summary>
        /// 获取已注册服务的实例
        /// </summary>
        /// <typeparam name="T">要获取的服务类型</typeparam>
        /// <returns>已注册服务的实例</returns>
        /// <exception cref="InvalidOperationException">当尚未初始化 App 实例时抛出</exception>
        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("App not initialized.");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 获取Web主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        /// <returns></returns>
        public static IWebHostEnvironment? WebHostEnvironment()
        {
            return _webHostEnvironment;
        }

        /// <summary>
        /// 获取本机 IPv4 地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddressToIPv4()
        {
            return _httpContext?.HttpContext?.Connection.LocalIpAddress?.MapToIPv4()?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 获取本机 IPv6 地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddressToIPv6()
        {
            return _httpContext?.HttpContext?.Connection.LocalIpAddress?.MapToIPv6()?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 获取远程 IPv4 地址
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteIpAddressToIPv4()
        {
            return _httpContext?.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4()?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 获取远程 IPv6 地址
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteIpAddressToIPv6()
        {
            return _httpContext?.HttpContext?.Connection.RemoteIpAddress?.MapToIPv6()?.ToString() ?? string.Empty;
        }
    }
}
