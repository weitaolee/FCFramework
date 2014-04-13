using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FC.Framework.Utilities;

namespace FC.Framework
{
    public class EventBus
    {
        private static ThreadLocal<List<Action>> _uncommittedActions = new ThreadLocal<List<Action>>(() => new List<Action>());

        public static void Apply<TEvent>(TEvent @event)
            where TEvent : IDomainEvent
        {
            Check.Argument.IsNotNull(@event, "event");

            FCFramework.DefaultEventBus.Publish(@event);

            lock (_uncommittedActions)
            {
                _uncommittedActions.Value.Add(() =>
                {
                    FCFramework.DefaultEventBus
                               .Publish<TEvent>(@event, EventDispatchStrategy.OnCommitted);
                });
            }
        }

        public static IEnumerable<Action> GetUncommitedActions()
        {
            return _uncommittedActions.Value;
        }

        public static void ClearUncommitedActions()
        {
            lock (_uncommittedActions)
            {
                _uncommittedActions.Value.Clear();
            }
        }



    }
}
