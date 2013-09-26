using System;
using Infrastruktur.EventSourcing;

namespace Modell.Kunden
{

    public sealed class KundeRepository : EventStoreBasedRepository
    {

        public KundeRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Kunde Retrieve(Guid aggregateId)
        {
            return new Kunde(new KundenProjektion(aggregateId, () => History(aggregateId)), e => Publish(aggregateId, e));
        }


    }
}
