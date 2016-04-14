using System;
using Infrastruktur.EventSourcing;

namespace Modell.Warenwirtschaft
{

    public sealed class ProduktRepository : EventStoreBasedRepository
    {

        public ProduktRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override AggregateEvents Validator
        {
            get { return Produkt.AggregateEvents; }
        }

        public Produkt Retrieve(Guid aggregateId)
        {
            return new Produkt(new ProduktProjektion(aggregateId, () => History(aggregateId)), e => Publish(aggregateId, e));
        }


    }


    public sealed class LagerRepository : EventStoreBasedRepository
    {

        public LagerRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override AggregateEvents Validator
        {
            get { return Lagerposten.AggregateEvents; }
        }

        public Lagerposten Retrieve(Guid lager, Guid produkt)
        {
            return new Lagerposten(lager, produkt, new LagerProjektion(() => History(lager)), e => Publish(lager, e));
        }


    }
}
