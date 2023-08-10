# 晨星博客 - api
[![sdk](https://img.shields.io/badge/sdk-6.0.21-d.svg)](#)  
-------------------------------
`MorningStar.Api`：表现层  
`MorningStar.Service`：服务层  
`MorningStar.Repository`：仓储层  
`MorningStar.Common`：公共层  
`MorningStar.Extension`：扩展层  
`MorningStar.Model`：模型层  

#### 框架模块：  
- [x] 采用`仓储+服务+接口`的形式封装框架；
- [x] 异步`async/await`开发；
- [x] 接入国产数据库ORM组件 ——`SqlSugar`，封装数据库操作，支持级联操作；
- [x] 实现项目启动，自动生成种子数据`CodeFirst` ✨； 

中间件模块：
- [x] 提供`Redis`做缓存处理；
- [x] 提供`MemoryCache`做缓存处理；
- [x] 使用`Swagger`做api接口文档；
- [x] 使用`MiniProfiler`做接口性能分析 ✨；
- [x] 使用`Autofac`做依赖注入容器，并提供批量服务注入 ✨；
- [x] 使用`AutoMapper`处理对象映射；
- [x] 支持`CORS`跨域；
- [x] 封装`JWT`做鉴权；
- [x] 添加`IpRateLimiting`做 API 限流处理；

微服务模块：
- [x] 可配合`Docker`、`Docker-Compose`实现容器化；