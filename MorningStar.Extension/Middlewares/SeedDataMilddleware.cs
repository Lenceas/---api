using MorningStar.Service;
using SqlSugar;

namespace MorningStar.Extension
{
    /// <summary>
    /// Sqlsugar CodeFirst 数据种子
    /// </summary>
    public static class SeedDataMilddleware
    {
        /// <summary>
        /// 生成【Sqlsugar CodeFirst】数据种子
        /// </summary>
        /// <param name="app"></param>
        public static void UseSeedDataMilddleware(this IApplicationBuilder app)
        {
            try
            {
                var db = app.ApplicationServices.GetService<ISqlSugarClient>() ?? throw new Exception("未获取到SqlSugarScope实例！");

                Console.WriteLine();
                Console.WriteLine("************ 开始自动初始化数据 **********");
                Console.WriteLine();

                Console.WriteLine("开始数据库连接检测...");
                if (db.CopyNew().DbMaintenance.CreateDatabase())
                    Console.WriteLine("数据库连接成功！");

                Console.WriteLine();
                Console.WriteLine("开始初始化表数据...");
                Console.WriteLine();

                #region 测试表 TestEntity
                Console.WriteLine($"开始初始化 {CommonHelper.GetClassDescription(typeof(TestEntity))} {nameof(TestEntity)} 数据...");
                db.CopyNew().CodeFirst.SetStringDefaultLength(255).InitTables(typeof(TestEntity));
                new TestService().InitDatas();
                Console.WriteLine($"{CommonHelper.GetClassDescription(typeof(TestEntity))} {nameof(TestEntity)} 数据初始化成功！");
                #endregion

                Console.WriteLine();
                Console.WriteLine("表数据初始化完成！");
                Console.WriteLine();

                Console.WriteLine("************ 自动初始化数据完成 **********");
            }
            catch (Exception ex)
            {
                throw new Exception("数据种子：【SqlSugar CodeFirst】生成错误：" + ex.Message);
            }

        }
    }
}
