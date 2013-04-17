using System;

namespace Infrastruktur.Common
{
    public abstract class Ereignis
    {
        public Guid EventSource { get; set; }
    }

    public sealed class Ereignis<T> : Ereignis where T : class
    {
        public T Daten { get; set; }
        public override string ToString()
        {
            return Daten.ToString();
        }
    }
}
