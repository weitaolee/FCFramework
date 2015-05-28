using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    public class FCFramework
    {
        public static FCFramework Instance { get; private set; }

        public static IEventBus DefaultEventBus { get { return IoC.Resolve<IEventBus>(); } }

        public static ICommandBus DefaultCommandBus { get { return IoC.Resolve<ICommandBus>(); } }

        private FCFramework() { }

        public static FCFramework Initialize()
        {
            if (Instance != null)
            {
                throw new Exception("Could not initialize twice");
            }

            Instance = new FCFramework();                                        

            return Instance;
        }

        public void UseDefaultJsonSerializer()
        {
            IoC.Register<IJsonSerializer, JsonSerializer>(LifeStyle.Singleton);
        }
    }
}
