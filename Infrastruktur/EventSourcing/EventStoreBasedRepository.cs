using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public IEnumerable<Ereignis> History(Guid aggregateId)
        {
            return Validator.Filter(_unitOfWork.History(aggregateId), aggregateId);
        }

        protected abstract AggregateEvents Validator { get; }

        protected void Publish(Guid aggregate, Ereignis ereignis)
        {
            if (!Validator.IsAffected(ereignis, aggregate)) throw new Exception("Events you don't consume, not publish you should!");
            _unitOfWork.Publish(ereignis);
        }

    }
}
