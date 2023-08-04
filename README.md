# 晨星博客 - api
[![sdk](https://img.shields.io/badge/sdk-6.0.20-d.svg)](#)  
-------------------------------
`MorningStar.Api`：表现层  
`MorningStar.Service`：服务层  
`MorningStar.Repository`：仓储层  
`MorningStar.Infrastructure`：基础设施层  
`MorningStar.Model`：领域模型层  
`MorningStar.Test`：单元测试层  

#### 框架模块：  
- [x] 采用`仓储+服务+接口`的形式封装框架；
- [x] 异步`async/await`开发；

中间件模块：
- [x] 使用`Swagger`做api接口文档；
- [x] 使用`Autofac`做依赖注入容器，并提供批量服务注入 ✨；
- [x] 使用`JWT`做鉴权；

微服务模块：
- [x] 可配合`Docker`、`Docker-Compose`实现容器化；