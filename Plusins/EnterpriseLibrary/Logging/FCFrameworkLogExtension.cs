namespace FC.Framework.EnterpriseLibrary
{
    public static class FCFrameworkLogExtension
    {
        public static FCFramework UseEnterpriseLibraryLog(this FCFramework framework, int frameToSkip = 4)
        {
            IoC.Register<ILog>(new EnterpriseLog(frameToSkip));

            return framework;
        }
    }
}
