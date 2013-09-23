using System;
using Infrastruktur.EventSourcing;

namespace Modell.Bestellwesen
{

    public sealed class AuftragRepository : EventStoreBasedRepository
    {

    public AuftragRepository(Guid aggregateId, UnitOfWork unitOfWork) : base(aggregateId, unitOfWork)
        {
        }

        public Auftrag Retrieve()
        {
            return new Auftrag(new AuftragProjektion(History), Publish);
        }

        
    }
}
