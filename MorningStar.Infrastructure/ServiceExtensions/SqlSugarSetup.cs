using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Reflection;

namespace MorningStar.Infrastructure
{
    /// <summary>
    /// SqlSugar 容器服务
    /// </summary>
    public static class SqlSugarSetup
    {
        /// <summary>
        /// 注册【SqlSugar】容器服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddSqlSugarSetup(this IServiceCollection services)
        {
            var configConnection = new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = AppSettings.Get("DataBase:Mysql"),
                IsAutoCloseConnection = true,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    EntityNameService = (type, entity) =>
                    {
                        // 全局AOP全部禁止删除列(CodeFirst)
                        //entity.IsDisabledDelete = true;
                        // 全局AOP全部禁止更新+删除(CodeFirst) - 比上面优先级更高
                        entity.IsDisabledUpdateAll = true;
                    },
                    EntityService = (c, p) =>
                    {
                        // 建表技巧：自动Nullable 
                        if (p.IsPrimarykey == false && new NullabilityInfoContext().Create(c).WriteState is NullabilityState.Nullable)
                        {
                            p.IsNullable = true;
                        }
                        // 兼容 mysql longtext 字段类型 只需要在实体属性上面加是：[SugarColumn(ColumnDataType = "varchar(max)")]
                        if (p.DataType == "varchar(max)")
                        {
                            p.DataType = "longtext";
                        }
                    }
                }
            };

            var sqlSugar = new SqlSugarScope(configConnection,
                db =>
                {
                    // 单例参数配置，所有上下文生效
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        // 输出sql
                        // Console.WriteLine(sql);
                    };
                });

            // 这边是SqlSugarScope用AddSingleton
            services.AddSingleton(sqlSugar);

            Console.WriteLine("容器服务：【SqlSugar】已注册！");
        }
    }
}
