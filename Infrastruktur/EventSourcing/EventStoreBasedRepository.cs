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

        protected void AffectedBy<TEreignis>(Func<TEreignis, Guid> discriminator) where TEreignis : class
        {

            _filters.Add(typeof(TEreignis), (ereignis, aggregate) => discriminator(((Ereignis<TEreignis>)ereignis).Daten) == aggregate);
        }

        protected void AffectedBy<TEreignis>(Func<TEreignis, Guid, bool> filter) where TEreignis:class
        {
            _filters.Add(typeof (TEreignis), (ereignis, aggregate) => filter(((Ereignis<TEreignis>) ereignis).Daten, aggregate));
        }

        public IEnumerable<Ereignis> History(Guid aggregateId)
        {
            return _unitOfWork.History(aggregateId);
        }

        protected void Publish(Guid aggregate, Ereignis ereignis)
        {
            if (!_filters[ereignis.DatenObjekt.GetType()](ereignis, aggregate)) throw new Exception("You shouldn't emit events on which you don't depend.");
            _unitOfWork.Publish(ereignis);
        }

    }
}
