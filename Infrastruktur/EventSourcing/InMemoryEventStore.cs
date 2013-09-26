using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public class InMemoryEventStore : EventStore
    {
        private readonly List<Ereignis> _worldhistory = new List<Ereignis>();

        public void Store(IList<Ereignis> daten)
        {
            _worldhistory.AddRange(daten);
            foreach (var e in daten)
            {
                System.Diagnostics.Trace.WriteLine(e);
                OnNewEvent(e);
            }
        }

        public IEnumerable<Ereignis> History { get { return _worldhistory.ToList(); } }
        public Func<Guid, IEnumerable<Ereignis>> Stream(Func<IEnumerable<Ereignis>, Guid, IEnumerable<Ereignis>> filter)
        {
            return id => filter(History, id);
        }

        public void Subscribe(Action<Ereignis> observer)
        {
            NewEvent += observer;
        }

        private event Action<Ereignis> NewEvent;

        protected virtual void OnNewEvent(Ereignis obj)
        {
            Action<Ereignis> handler = NewEvent;
            if (handler != null) handler(obj);
        }
    }
}
