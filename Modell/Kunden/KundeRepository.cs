using System;
using Infrastruktur.EventSourcing;

namespace Modell.Kunden
{

    public sealed class KundeRepository : EventStoreBasedRepository
    {

        public KundeRepository(Guid aggregateId, UnitOfWork unitOfWork)
            : base(aggregateId, unitOfWork)
        {
        }

        public Kunde Retrieve()
        {
            return new Kunde(new KundenProjektion(AggregateId, History), Publish);
        }


    }
}
