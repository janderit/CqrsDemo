using System;

namespace Infrastruktur.Common
{
    public abstract class Ereignis
    {
        internal abstract object DatenObjekt { get; }
    }

    public sealed class Ereignis<T> : Ereignis
    {
        internal override object DatenObjekt { get { return Daten; } }
        public T Daten { get; set; }
        public override string ToString()
        {
            return Daten.ToString();
        }
    }
}
