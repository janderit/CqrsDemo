using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public sealed class UnitOfWork
    {
        private readonly EventStore _store;

        public UnitOfWork(EventStore store)
        {
            _store = store;
        }

        private readonly List<Ereignis> _uncommitted = new List<Ereignis>();

        internal IEnumerable<Ereignis> History(Guid eventsource)
        {
            return _store.Retrieve(eventsource)
                         .Concat(_uncommitted.Where(e => e.EventSource == eventsource));
        }

        internal void Publish(Guid eventsource, Ereignis ereignis)
        {
            ereignis.EventSource = eventsource;
            _uncommitted.Add(ereignis);
        }

        public IEnumerable<Ereignis> Commit()
        {
            var result = _uncommitted.ToList();
            _store.Store(result);
            _uncommitted.Clear();
            return result;
        }
        

    }
}
