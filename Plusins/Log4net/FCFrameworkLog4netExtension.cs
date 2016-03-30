namespace FC.Framework.Log4net
{
    public static class FCFrameworkLog4netExtension
    {
        /// <summary>
        /// 使用log4net作为FCFramework的日志组件
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="configFile">log4net配置文件名称，默认为log4net.config</param>
        /// <returns>FCFramework</returns>
        public static FCFramework UseLog4net(this FCFramework framework, string configFile = "log4net.config")
        {
            IoC.Register<ILog>(new Log4netLogger(configFile));

            return framework;
        }
    }
}
