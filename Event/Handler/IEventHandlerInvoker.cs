using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FC.Framework.DynamicReflection;

namespace FC.Framework
{
    public interface IEventHandlerInvoker
    {
        void Invoke(object handler, MethodInfo handleMethod, object arg);
    }

    public class FastEventHandlerInvoker : IEventHandlerInvoker
    {
        private Dictionary<int, Proc<object, object>> cacheMethods = new Dictionary<int, Proc<object, object>>();

        public void Invoke(object handler, MethodInfo handleMethod, object arg)
        {
            Proc<object, object> dynamicMethod;
            int hashcode = handleMethod.GetHashCode();
            if (!cacheMethods.ContainsKey(hashcode))
            {
                dynamicMethod = Dynamic<Object>.Instance.Procedure.Explicit<object>.CreateDelegate(handleMethod);
                cacheMethods.Add(hashcode, dynamicMethod);
            }
            else
                dynamicMethod = cacheMethods[hashcode];

            dynamicMethod.Invoke(handler, arg);
        }
    }
}
