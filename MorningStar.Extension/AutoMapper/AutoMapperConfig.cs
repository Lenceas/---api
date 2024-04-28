namespace MorningStar.Extension
{
    /// <summary>
    /// AutoMapper 配置
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoMapperConfig()
        {
            #region BaseEntity >> BaseDTOModel
            CreateMap<BaseEntity, BaseDTOModel>()
                .ForMember(_ => _.ReportDate,
                _ => _.MapFrom(
                    src => src.ReportDate.ToLocalTime().ToString("yyyy-MM-dd")))
                .ForMember(_ => _.MTime, _ => _.MapFrom(
                    src => src.MTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(_ => _.CTime, _ => _.MapFrom(
                    src => src.CTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")));
            #endregion
        }
    }
}
