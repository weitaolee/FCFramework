using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FC.Framework.DynamicReflection;
using System.Diagnostics;
namespace FC.Framework
{
    public interface IEventHandlerActivator
    {
        object CreateInstance(Type type);
    }

    /// <summary>
    /// 事件处理器 Activor
    /// <remarks>因部分事件处理器构造函数不同，且需要IoC注入，所以依赖原始的IoC创建最为合适，暂时不考虑Emit</remarks> 
    /// </summary>
    public class EventHandlerActivator : IEventHandlerActivator
    {
        //  private Dictionary<int, Proc<object, object>> handlerConstructors = new Dictionary<int, Proc<object, object>>();
        public object CreateInstance(Type type)
        {
            try
            {
                return IoC.Resolve(type);
            }
            catch (IoCException)
            {
                if (type.IsDefined(typeof(ComponentAttribute), false))
                    IoC.Register(type, LifeStyle.Singleton);
                else
                    IoC.Register(type, LifeStyle.Transient);
                return IoC.Resolve(type);
            }
        }
    }
}
