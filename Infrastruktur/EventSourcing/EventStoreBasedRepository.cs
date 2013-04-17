using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public abstract class EventStoreBasedRepository
    {
        protected readonly Guid AggregateId;
        private readonly EventStore _store;
        private readonly List<Ereignis> _pastHistory;

        protected EventStoreBasedRepository(Guid aggregateId, EventStore store)
        {
            if (aggregateId == Guid.Empty)
                throw new InvalidOperationException("Aggregat ohne ID kann nicht erzeugt werden.");

            AggregateId = aggregateId;
            _store = store;
            _pastHistory = _store.Retrieve(aggregateId).ToList();
        }

        private readonly List<Ereignis> _uncommitted = new List<Ereignis>();

        protected IEnumerable<Ereignis> History()
        {
            return _pastHistory.Concat(_uncommitted);
        }

        protected void Publish(Ereignis ereignis)
        {
            ereignis.EventSource = AggregateId;
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
