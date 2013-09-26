using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public sealed class AggregateEvents
    {
        private readonly Dictionary<Type, Func<Ereignis, Guid, bool>> _filters = new Dictionary<Type, Func<Ereignis, Guid, bool>>();

        public AggregateEvents AggregateIsAffectedBy<TEreignis>(Func<TEreignis, Guid> discriminator) where TEreignis : class
        {
            _filters.Add(typeof(TEreignis), (ereignis, aggregate) => discriminator(((Ereignis<TEreignis>)ereignis).Daten) == aggregate);
            return this;
        }

        public AggregateEvents AggregateIsAffectedBy<TEreignis>(Func<TEreignis, Guid, bool> filter) where TEreignis : class
        {
            _filters.Add(typeof(TEreignis), (ereignis, aggregate) => filter(((Ereignis<TEreignis>)ereignis).Daten, aggregate));
            return this;
        }

        public bool IsAffected(Ereignis ereignis, Guid aggregate)
        {
            Func<Ereignis, Guid, bool> filter;
            if (!_filters.TryGetValue(ereignis.DatenObjekt.GetType(), out filter)) return false;
            return filter(ereignis, aggregate);
        }

        public IEnumerable<Ereignis> Filter(IEnumerable<Ereignis> source, Guid aggregate)
        {
            return source.Where(e => IsAffected(e, aggregate));
        }

    }
}