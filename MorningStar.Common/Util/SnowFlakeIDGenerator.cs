using SharpAbp.Abp.Snowflakes;

namespace MorningStar.Common
{
    /// <summary>
    /// 雪花算法ID生成器（获取唯一值的场景可以使用）
    /// </summary>
    public static class SnowFlakeIDGenerator
    {
        private static readonly Snowflake _snowflake = new(
            long.Parse(AppSettings.Get("SnowFlakeID:SnowFlakeWorkerIdBits") ?? "24"),
            long.Parse(AppSettings.Get("SnowFlakeID:SnowFlakeTwepoch") ?? "24")
            );

        /// <summary>
        /// 获取下一个LongID
        /// </summary>
        /// <returns></returns>s
        public static long NextLongID
        {
            get
            {
                return _snowflake.NextId();
            }
        }
    }
}
