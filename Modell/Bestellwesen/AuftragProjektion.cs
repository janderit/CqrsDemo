using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;

namespace Modell.Bestellwesen
{
    public sealed class AuftragProjektion
    {

        public static IEnumerable<Guid> AlleIDs(IEnumerable<Ereignis> history)
        {
            return history.OfType<Ereignis<AuftragWurdeErfasst>>().Select(_ => _.Daten.Kunde).ToList();
        }

        private readonly Func<IEnumerable<Ereignis>> _history;

        public AuftragProjektion(Func<IEnumerable<Ereignis>> history)
        {
            _history = history;
        }


        public Guid Produkt
        {
            get
            {
                return _history().OfType<Ereignis<AuftragWurdeErfasst>>().Single().Daten.Produkt;
            }
        }

        public Guid Kunde
        {
            get
            {
                return _history().OfType<Ereignis<AuftragWurdeErfasst>>().Single().Daten.Kunde;
            }
        }

        public int Menge
        {
            get
            {
                return _history().OfType<Ereignis<AuftragWurdeErfasst>>().Single().Daten.Menge;
            }
        }

        public bool Erfasst
        {
            get
            {
                return _history().OfType<Ereignis<AuftragWurdeErfasst>>().Any();
            }
        }

        public bool Erfuellt
        {
            get
            {
                return _history().OfType<Ereignis<AuftragWurdeErfuellt>>().Any();
            }
        }

    }
}
