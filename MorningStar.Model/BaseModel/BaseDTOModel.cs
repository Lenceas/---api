namespace MorningStar.Model
{
    /// <summary>
    /// DTOModel基类
    /// </summary>
    public class BaseDTOModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 排序下标
        /// </summary>
        public short OrderIndex { get; set; }

        /// <summary>
        /// 报表日期 yyyy-MM-dd
        /// </summary>
        public string ReportDate { get; set; } = string.Empty;

        /// <summary>
        /// 修改人ID
        /// </summary>
        public long? MID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string MName { get; set; } = string.Empty;

        /// <summary>
        /// 修改时间 yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string MTime { get; set; } = string.Empty;

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long? CID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CName { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间 yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string CTime { get; set; } = string.Empty;
    }
}
