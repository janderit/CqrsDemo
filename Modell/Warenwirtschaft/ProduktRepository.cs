using System;
using Infrastruktur.EventSourcing;
using Modell.Bestellwesen;

namespace Modell.Warenwirtschaft
{

    public sealed class ProduktRepository : EventStoreBasedRepository
    {

        public ProduktRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Produkt Retrieve(Guid aggregateId)
        {
            return new Produkt(new ProduktProjektion(aggregateId, () => History(aggregateId)), e => Publish(aggregateId, e));
        }

        
    }
}
