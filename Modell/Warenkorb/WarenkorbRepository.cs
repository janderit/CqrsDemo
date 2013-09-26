using System;
using Infrastruktur.EventSourcing;

namespace Modell.Warenkorb
{

    public sealed class WarenkorbRepository : EventStoreBasedRepository
    {

        public WarenkorbRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {            
        }

        protected override AggregateEvents Validator
        {
            get { return Warenkorb.AggregateEvents; }
        }

        public Warenkorb Retrieve(Guid aggregateId)
        {
            return new Warenkorb(new WarenkorbProjektion(aggregateId, () => History(aggregateId)), e => Publish(aggregateId, e));
        }
    }
}
