namespace MorningStar.Model
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键", CreateTableFieldSort = -99)]
        public long ID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDescription = "备注", IsNullable = true, CreateTableFieldSort = 91)]
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 排序下标, 越小越靠前 (只针对同级有效, 不夸层)
        /// </summary>
        [SugarColumn(ColumnDescription = "排序下标", CreateTableFieldSort = 92)]
        public short OrderIndex { get; set; } = 999;

        /// <summary>
        /// 报表日期
        /// </summary>
        [SugarColumn(ColumnDescription = "报表日期", CreateTableFieldSort = 93)]
        public DateTime ReportDate { get; set; } = DateTime.Today.ToUniversalTime();

        /// <summary>
        /// 修改人ID
        /// </summary>
        [SugarColumn(ColumnDescription = "修改人ID", CreateTableFieldSort = 94)]
        public long MID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnDescription = "修改人", CreateTableFieldSort = 95)]
        public string MName { get; set; } = string.Empty;

        /// <summary>
        /// 修改时间(UTC)
        /// </summary>
        [SugarColumn(InsertSql = "UTC_TIMESTAMP()", UpdateSql = "UTC_TIMESTAMP()", ColumnDescription = "修改时间(UTC)", CreateTableFieldSort = 96)]
        public DateTime MTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 创建人ID
        /// </summary>
        [SugarColumn(ColumnDescription = "创建人ID", CreateTableFieldSort = 97)]
        public long CID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnDescription = "创建人", CreateTableFieldSort = 98)]
        public string CName { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间(UTC)
        /// </summary>
        [SugarColumn(InsertSql = "UTC_TIMESTAMP()", ColumnDescription = "创建时间(UTC)", CreateTableFieldSort = 99)]
        public DateTime CTime { get; set; } = DateTime.UtcNow;
    }
}