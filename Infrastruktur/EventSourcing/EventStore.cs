using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public interface EventStore
    {
        void Store(IList<Ereignis> daten);
        IEnumerable<Ereignis> History { get; }
        void Subscribe(Action<Ereignis> observer);
    }
}
