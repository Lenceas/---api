using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MorningStar.Model
{
    /// <summary>
    /// Mongo实体基类
    /// </summary>
    public class BaseMongoEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.Int64), Description("主键"), BsonElement(Order = -99)]
        public long ID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注"), BsonElement(Order = 92)]
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 排序下标（越小越靠前,只针对同级有效,不跨层）
        /// </summary>
        [Description("排序下标"), BsonElement(Order = 92)]
        public int OrderIndex { get; set; } = 999;

        /// <summary>
        /// 报表日期
        /// </summary>
        [Description("报表日期"), BsonElement(Order = 93)]
        public DateTime ReportDate { get; set; } = DateTime.Today.ToUniversalTime();

        /// <summary>
        /// 修改人ID
        /// </summary>
        [Description("修改人ID"), BsonElement(Order = 94)]
        public long MID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Description("修改人"), BsonElement(Order = 95)]
        public string MName { get; set; } = string.Empty;

        /// <summary>
        /// 修改时间(UTC)
        /// </summary>
        [Description("修改时间(UTC)"), BsonElement(Order = 96)]
        public DateTime MTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Description("创建人ID"), BsonElement(Order = 97)]
        public long CID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人"), BsonElement(Order = 98)]
        public string CName { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间(UTC)
        /// </summary>
        [Description("创建时间(UTC)"), BsonElement(Order = 99)]
        public DateTime CTime { get; set; } = DateTime.UtcNow;
    }
}