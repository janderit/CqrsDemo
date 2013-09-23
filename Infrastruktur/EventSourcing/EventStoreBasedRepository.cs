using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public abstract class EventStoreBasedRepository
    {
        protected readonly Guid AggregateId;
        private readonly UnitOfWork _unitOfWork;

        protected EventStoreBasedRepository(Guid aggregateId, UnitOfWork unitOfWork)
        {
            if (aggregateId == Guid.Empty)
                throw new InvalidOperationException("Aggregat ohne ID kann nicht erzeugt werden.");

            AggregateId = aggregateId;
            _unitOfWork = unitOfWork;
        }

        protected IEnumerable<Ereignis> History()
        {
            return _unitOfWork.History(AggregateId);
        }

        protected void Publish(Ereignis ereignis)
        {
            _unitOfWork.Publish(AggregateId, ereignis);
        }

    }
}
