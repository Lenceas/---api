using MongoDB.Driver;
using MorningStar.Service;
using SqlSugar;

namespace MorningStar.Extension
{
    /// <summary>
    /// CodeFirst 生成数据
    /// </summary>
    public static class SeedDataMilddleware
    {
        /// <summary>
        /// CodeFirst 生成数据
        /// </summary>
        /// <param name="app"></param>
        /// <param name="log"></param>
        /// <exception cref="Exception"></exception>
        public static void UseSeedDataMilddleware(this IApplicationBuilder app, Serilog.ILogger log)
        {
            try
            {
                var db = app.ApplicationServices.GetService<ISqlSugarClient>() ?? throw new Exception("未获取到ISqlSugarClient实例！");
                var mongo_db = app.ApplicationServices.GetService<IMongoDatabase>() ?? throw new Exception("未获取到IMongoDatabase实例！");

                Console.WriteLine();
                log.Information("************ 开始自动初始化数据 **********");
                Console.WriteLine();

                log.Information("开始 mysql 数据库连接检测...");
                if (db.DbMaintenance.CreateDatabase())
                    log.Information("mysql 数据库连接成功！");

                log.Information("开始 mongo 数据库连接检测...");
                if (mongo_db.ListCollectionNames().ToList().Count >= 0)
                    log.Information("mongo 数据库连接成功！");

                Console.WriteLine();
                log.Information("开始初始化表数据...");
                Console.WriteLine();

                #region mysql

                #region 测试MySql表 TestMySqlEntity

                log.Information($"开始初始化 {CommonHelper.GetClassDescription(typeof(TestMySqlEntity))} {nameof(TestMySqlEntity)} 数据...");
                db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(TestMySqlEntity));
                new TestMySqlService().InitDatas();
                log.Information($"{CommonHelper.GetClassDescription(typeof(TestMySqlEntity))} {nameof(TestMySqlEntity)} 数据初始化成功！");

                #endregion 测试MySql表 TestMySqlEntity

                #endregion mysql

                #region mongo

                #region 测试Mongo表 TestMongoEntity

                log.Information($"开始初始化 {CommonHelper.GetClassDescription(typeof(TestMongoEntity))} {nameof(TestMongoEntity)} 数据...");
                new TestMongoService().InitDatas();
                log.Information($"{CommonHelper.GetClassDescription(typeof(TestMongoEntity))} {nameof(TestMongoEntity)} 数据初始化成功！");

                #endregion 测试Mongo表 TestMongoEntity

                #region 用户表 UserMongoEntity

                log.Information($"开始初始化 {CommonHelper.GetClassDescription(typeof(UserMongoEntity))} {nameof(UserMongoEntity)} 数据...");
                new UserService().InitDatas();
                log.Information($"{CommonHelper.GetClassDescription(typeof(UserMongoEntity))} {nameof(UserMongoEntity)} 数据初始化成功！");

                #endregion 用户表 UserMongoEntity

                #endregion mongo

                Console.WriteLine();
                log.Information("表数据初始化完成！");
                Console.WriteLine();

                log.Information("************ 自动初始化数据完成 **********");
            }
            catch (Exception ex)
            {
                throw new Exception("CodeFirst：数据生成错误：" + ex.Message);
            }
        }
    }
}