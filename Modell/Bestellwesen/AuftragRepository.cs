using System;
using Infrastruktur.EventSourcing;

namespace Modell.Bestellwesen
{

    public sealed class AuftragRepository : EventStoreBasedRepository
    {

    public AuftragRepository(Guid aggregateId, EventStore store) : base(aggregateId, store)
        {
        }

        public Auftrag Retrieve()
        {
            return new Auftrag(new AuftragProjektion(History), Publish);
        }

        
    }
}
