using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api;
using Host;
using Infrastruktur.EventSourcing;

namespace Spezifikation
{
    public abstract class Akzeptanztest
    {
        protected CqrsApi TestInstanz(Action<EventStore> hook = null)
        {
            var store = new InMemoryEventStore();
            var host = new CqrsHost(store);
            if (hook != null) hook(store);
            return new CqrsApi(host);
        }
    }
}
