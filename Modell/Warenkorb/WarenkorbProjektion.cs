using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;

namespace Modell.Warenkorb
{
    public sealed class WarenkorbProjektion
    {

        public static IEnumerable<Guid> AlleIDs(IEnumerable<Ereignis> history)
        {
            return history.OfType<Ereignis<WarenkorbWurdeEroeffnet>>().Select(_ => _.EventSource).ToList();
        }

        private readonly Func<IEnumerable<Ereignis>> _history;

        public WarenkorbProjektion(Guid id, Func<IEnumerable<Ereignis>> history)
        {
            _history = history;
            Id = id;
        }

        public readonly Guid Id;

        public Guid Kunde
        {
            get
            {
                return _history().OfType<Ereignis<WarenkorbWurdeEroeffnet>>().Single().Daten.Kunde;
            }
        }

        public bool Leer
        {
            get { return !_history().OfType<Ereignis<ArtikelWurdeZuWarenkorbHinzugefuegt>>().Any(); }
        }

    }
}
