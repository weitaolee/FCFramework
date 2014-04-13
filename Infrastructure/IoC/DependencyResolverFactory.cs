

namespace FC.Framework
{
    using System;
    using FC.Framework.Utilities;
    /// <summary>
    /// 依赖反转工厂
    /// </summary>
    public class DependencyResolverFactory : IDependencyResolverFactory
    {
        private readonly Type _resolverType;

        public DependencyResolverFactory(string resolverType)
        {
            Check.Argument.IsNotEmpty(resolverType, "resolverType");
            _resolverType = Type.GetType(resolverType, true, true);
        }

        public DependencyResolverFactory(Type resolverType)
        {
            Check.Argument.IsNotNull(resolverType, "resolverType");

            _resolverType = resolverType;
        }


        public IDependencyResolver CreateInstance()
        {
            return Activator.CreateInstance(_resolverType) as IDependencyResolver;
        }
    }
}
