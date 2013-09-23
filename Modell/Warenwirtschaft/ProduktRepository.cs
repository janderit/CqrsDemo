using System;
using Infrastruktur.EventSourcing;
using Modell.Bestellwesen;

namespace Modell.Warenwirtschaft
{

    public sealed class ProduktRepository : EventStoreBasedRepository
    {

        public ProduktRepository(Guid aggregateId, UnitOfWork unitOfWork)
            : base(aggregateId, unitOfWork)
        {
        }

        public Produkt Retrieve()
        {
            return new Produkt(new ProduktProjektion(AggregateId, History), Publish);
        }

        
    }
}
