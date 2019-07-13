FCFramework
===========

A DDD-EDA Framework

这是一个DDD的开发框架，在自5年前创建至今，经历了多个线上项目的实践，已非常稳定成熟。
最近一次更新是增加了Redis的缓存实现 2019-07-13；


接下来这个框架将不再更新，此框架仍然可以使用在NET FRAMEWORK 4.X之下，作为传统的DDD开发使用；

FCFramework框架开发于MVC3的背景之下，FCFramework实现包含了 IOC LOG CACHE等web开发中需要的基础组件顶层接口，并基于此接口实现了诸如Autofac,log4net,memcache,redis的实现，作为默认实现，任何人都可以基于高层接口实现自己的LOG\CACHE\IOC。

由于目前NET CORE项目的快速发展，框架已包含 IOC/DI LOG CACHE的基础架构支持，且拥有一众三方组件的支持。因此，FCFramework实现的IOC\LOG\CACHE在新版本的NET CORE已有良好的替代实现。因此 FCFramework 将停止更新。


接下来会花一点时间，仅把此框架中关于DDD实现的部分，独立迁移到NET CORE上，用于支持在NET CORE之下的DDD开发。
敬请期待
