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
            get
            {
                var h = _history();
                return !h.OfType<Ereignis<ArtikelWurdeZuWarenkorbHinzugefuegt>>().Any();
            }
        }

    }
}
