using System;
using Infrastruktur.EventSourcing;

namespace Modell.Kunden
{

    public sealed class KundeRepository : EventStoreBasedRepository
    {

        public KundeRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            AffectedBy<KundeWurdeErfasst>(e => e.Kunde);
            AffectedBy<AnschriftWurdeGeaendert>(e => e.Kunde);
        }

        public Kunde Retrieve(Guid aggregateId)
        {
            return new Kunde(new KundenProjektion(aggregateId, () => History(aggregateId)), e => Publish(aggregateId, e));
        }


    }
}
