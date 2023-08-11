using MongoDB.Driver;
using MorningStar.Service;
using SqlSugar;

namespace MorningStar.Extension
{
    /// <summary>
    /// CodeFirst 数据种子
    /// </summary>
    public static class SeedDataMilddleware
    {
        /// <summary>
        /// 生成【CodeFirst】数据种子
        /// </summary>
        /// <param name="app"></param>
        public static void UseSeedDataMilddleware(this IApplicationBuilder app)
        {
            try
            {
                var db = app.ApplicationServices.GetService<ISqlSugarClient>() ?? throw new Exception("未获取到ISqlSugarClient实例！");
                var mongo_db = app.ApplicationServices.GetService<IMongoDatabase>() ?? throw new Exception("未获取到IMongoDatabase实例！");

                Console.WriteLine();
                Console.WriteLine("************ 开始自动初始化数据 **********");
                Console.WriteLine();

                Console.WriteLine("开始 mysql 数据库连接检测...");
                if (db.DbMaintenance.CreateDatabase())
                    Console.WriteLine("mysql 数据库连接成功！");

                Console.WriteLine("开始 mongo 数据库连接检测...");
                if (mongo_db.ListCollectionNames().ToList().Count >= 0)
                    Console.WriteLine("mongo 数据库连接成功！");

                Console.WriteLine();
                Console.WriteLine("开始初始化表数据...");
                Console.WriteLine();

                #region mysql

                #region 测试表 TestEntity
                Console.WriteLine($"开始初始化 {CommonHelper.GetClassDescription(typeof(TestEntity))} {nameof(TestEntity)} 数据...");
                db.CodeFirst.SetStringDefaultLength(255).InitTables(typeof(TestEntity));
                new TestService().InitDatas();
                Console.WriteLine($"{CommonHelper.GetClassDescription(typeof(TestEntity))} {nameof(TestEntity)} 数据初始化成功！");
                #endregion

                #endregion

                #region mongo

                #region 测试Mongo表 TestMongoEntity
                Console.WriteLine($"开始初始化 {CommonHelper.GetClassDescription(typeof(TestMongoEntity))} {nameof(TestMongoEntity)} 数据...");                
                new TestMongoService().InitDatas();
                Console.WriteLine($"{CommonHelper.GetClassDescription(typeof(TestMongoEntity))} {nameof(TestMongoEntity)} 数据初始化成功！");
                #endregion

                #endregion

                Console.WriteLine();
                Console.WriteLine("表数据初始化完成！");
                Console.WriteLine();

                Console.WriteLine("************ 自动初始化数据完成 **********");
            }
            catch (Exception ex)
            {
                throw new Exception("数据种子：【CodeFirst】生成错误：" + ex.Message);
            }

        }
    }
}
