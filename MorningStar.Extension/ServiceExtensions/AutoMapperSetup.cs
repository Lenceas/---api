namespace MorningStar.Extension
{
    /// <summary>
    /// AutoMapper 容器服务
    /// </summary>
    public static class AutoMapperSetup
    {
        /// <summary>
        /// 注册【AutoMapper】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            // 注册 AutoMapper 单例服务
            services.AddSingleton(mapper);
        }
    }
}