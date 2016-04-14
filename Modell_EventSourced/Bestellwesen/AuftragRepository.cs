using System;
using Infrastruktur.EventSourcing;

namespace Modell.Bestellwesen
{

    public sealed class AuftragRepository : EventStoreBasedRepository
    {

        public AuftragRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override AggregateEvents Validator
        {
            get { return Auftrag.AggregateEvents; }
        }

        public Auftrag Retrieve(Guid aggregateId)
        {
            return new Auftrag(new AuftragProjektion(() => History(aggregateId)), e => Publish(aggregateId, e));
        }

    }
}
