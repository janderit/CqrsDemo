using System;
using Infrastruktur.Common;

namespace Infrastruktur.EventSourcing
{
    public class AggregateRoot
    {
        private readonly Action<Ereignis> _publish;

        protected AggregateRoot(Action<Ereignis> eventSink)
        {
            _publish = eventSink;
        }

        protected void Publish<T>(T ereignis) where T:class
        {
            _publish(new Ereignis<T> {Daten = ereignis});
        }
    }
}