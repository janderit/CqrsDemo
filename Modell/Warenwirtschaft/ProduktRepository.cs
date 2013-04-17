using System;
using Infrastruktur.EventSourcing;
using Modell.Bestellwesen;

namespace Modell.Warenwirtschaft
{

    public sealed class ProduktRepository : EventStoreBasedRepository
    {

        public ProduktRepository(Guid aggregateId, EventStore store)
            : base(aggregateId, store)
        {
        }

        public Produkt Retrieve()
        {
            return new Produkt(new ProduktProjektion(AggregateId, History), Publish);
        }

        
    }
}
