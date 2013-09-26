using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public abstract class EventStoreBasedRepository
    {
        private readonly UnitOfWork _unitOfWork;

        protected EventStoreBasedRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected IEnumerable<Ereignis> History(Guid aggregateId)
        {
            return _unitOfWork.History(aggregateId);
        }

        protected void Publish(Guid aggregate, Ereignis ereignis)
        {
            _unitOfWork.Publish(aggregate, ereignis);
        }

    }
}
