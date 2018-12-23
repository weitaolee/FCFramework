

namespace FC.Framework.EnterpriseLibrary
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using System.Diagnostics;
    using System.Configuration;
    using System.IO;
    using System.Collections.ObjectModel;
    using FC.Framework.Utilities;
    using Unity.Resolution;
    using Unity;
    using Unity.Lifetime;
    using Unity.Exceptions;

    /// <summary>
    /// 基于企业库的IoC实现
    /// </summary>
    public class UnityDependencyResolver : DisposableResource, IDependencyResolver
    {

        private readonly IUnityContainer _container;


        public UnityDependencyResolver()
            : this(new UnityContainer())
        {

            // UnityConfigurationSection configuration = new ConfigurationManagerWrapper().GetSection<UnityConfigurationSection>("unity");
            var configFilePath = ConfigurationManagerWrapper.AppSettings["unityConfig"];

            var unityConfig = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + configFilePath);
            if (File.Exists(unityConfig))
            {
                var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = unityConfig };
                var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

                unitySection.Configure(_container);
            }
        }

        [DebuggerStepThrough]
        public UnityDependencyResolver(IUnityContainer container)
        {
            Check.Argument.IsNotNull(container, "container");

            _container = container;
        }

        public void Register<T>(LifeStyle lifeStyle = LifeStyle.Transient) where T : class
        {
            var lifeTimeManager = GetLifeTimeManager(lifeStyle);
            _container.RegisterType<T>(lifeTimeManager);
        }

        public void Register(Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Check.Argument.IsNotNull(typeImpl, "typeImpl");

            var lifeTimeManager = GetLifeTimeManager(lifeStyle);
            _container.RegisterType(typeImpl, lifeTimeManager);
        }

        public void Register(Type typeInterface, Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Check.Argument.IsNotNull(typeInterface, "typeInterface");
            Check.Argument.IsNotNull(typeImpl, "typeImpl");

            var lifeTimeManager = GetLifeTimeManager(lifeStyle);

            _container.RegisterType(typeInterface, typeImpl, lifeTimeManager);
        }

        public void Register(Type typeInterface, Type typeImpl, string name, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Check.Argument.IsNotNull(typeInterface, "typeInterface");
            Check.Argument.IsNotNull(typeImpl, "typeImpl");
            Check.Argument.IsNotEmpty(name, "name");

            var lifeTimeManager = GetLifeTimeManager(lifeStyle);

            _container.RegisterType(typeInterface, typeImpl, name, lifeTimeManager);
        }

        public void Register<TInterface, TImpl>(LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Register(typeof(TInterface), typeof(TImpl), lifeStyle);
        }

        public void Register<TInterface, TImpl>(string name, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Register(typeof(TInterface), typeof(TImpl), name, lifeStyle);
        }

        public void Register<T>(T instance) where T : class
        {
            Check.Argument.IsNotNull(instance, "instance");

            _container.RegisterInstance<T>(instance);
        }

        public void Register<T>(T instance, string name) where T : class
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotEmpty(name, "name");

            _container.RegisterInstance<T>(name, instance);
        }

        public T Resolve<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            try
            {
                return _container.Resolve<T>(name);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve<T>(_params);
                }
                else
                    return _container.Resolve<T>();
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve<T>(name, _params);
                }
                else
                    return _container.Resolve<T>(name);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(Dictionary<string, object> parameters)
        {
            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve<T>(_params);
                }
                else
                    return _container.Resolve<T>();
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(string name, Dictionary<string, object> parameters)
        {
            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve<T>(name, _params);
                }
                else
                    return _container.Resolve<T>(name);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }


        public object Resolve(Type type)
        {
            Check.Argument.IsNotNull(type, "type");
            try
            {
                return _container.Resolve(type);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, string name)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                return _container.Resolve(type, name);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, params KeyValuePair<string, object>[] parameters)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve(type, _params);
                }
                else
                    return _container.Resolve(type);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, string name, params KeyValuePair<string, object>[] parameters)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve(type, name, _params);
                }
                else
                    return _container.Resolve(type);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, Dictionary<string, object> parameters)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve(type, _params);
                }
                else
                    return _container.Resolve(type);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, string name, Dictionary<string, object> parameters)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    var _params = this.ConvertToOverrides(parameters);
                    return _container.Resolve(type, _params);
                }
                else
                    return _container.Resolve(type);
            }
            catch (ResolutionFailedException ex)
            {
                throw new IoCException(ex);
            }
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)ResolveAll(typeof(T));
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            IEnumerable<object> namedInstances = _container.ResolveAll(type);
            object unnamedInstance = null;

            try
            {
                unnamedInstance = _container.Resolve(type);
            }
            catch (ResolutionFailedException)
            {
                //When default instance is missing
            }

            if (Equals(unnamedInstance, null))
            {
                return namedInstances;
            }

            return new ReadOnlyCollection<object>(new List<object>(namedInstances) { unnamedInstance });
        }

        private LifetimeManager GetLifeTimeManager(LifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case LifeStyle.Transient:
                    return new TransientLifetimeManager();
                case LifeStyle.Singleton:
                    return new ContainerControlledLifetimeManager();
                case LifeStyle.PerHttpRequest:
                    return new UnityPerWebRequestLifetimeManager();
                case LifeStyle.PerThread:
                    return new PerThreadLifetimeManager();
                default:
                    return new TransientLifetimeManager();
            }
        }

        [DebuggerStepThrough]
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _container.Dispose();
                }

                base.Dispose(disposing);
                _disposed = true;
            }
        }

        private bool _disposed = false;

        #region 参数类型转换
        private ParameterOverrides ConvertToOverrides(params KeyValuePair<string, object>[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                ParameterOverrides _params = new ParameterOverrides(); ;
                foreach (var param in parameters)
                {
                    _params.Add(param.Key, param.Value);
                }
                return _params;
            }
            else
                return null;
        }

        private ParameterOverrides ConvertToOverrides(Dictionary<string, object> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                ParameterOverrides _params = new ParameterOverrides(); ;
                foreach (var param in parameters)
                {
                    _params.Add(param.Key, param.Value);
                }
                return _params;
            }
            else
                return null;
        }
        #endregion
    }
}
