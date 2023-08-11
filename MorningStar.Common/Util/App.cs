using Microsoft.Extensions.DependencyInjection;

namespace MorningStar
{
    /// <summary>
    /// 全局类，用于提供访问已注册服务的方法
    /// </summary>
    public static class App
    {
        private static IServiceProvider? _serviceProvider;

        /// <summary>
        /// 初始化全局 App 实例，必须在应用程序启动时调用
        /// </summary>
        /// <param name="serviceProvider">依赖注入容器</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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
    }
}
