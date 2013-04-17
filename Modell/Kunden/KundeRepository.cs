using System;
using Infrastruktur.EventSourcing;

namespace Modell.Kunden
{

    public sealed class KundeRepository : EventStoreBasedRepository
    {

        public KundeRepository(Guid aggregateId, EventStore store)
            : base(aggregateId, store)
        {
        }

        public Kunde Retrieve()
        {
            return new Kunde(new KundenProjektion(AggregateId, History), Publish);
        }


    }
}
