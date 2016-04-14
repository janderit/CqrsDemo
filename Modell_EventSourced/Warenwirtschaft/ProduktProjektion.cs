using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur;
using Infrastruktur.Common;
using Modell.Bestellwesen;

namespace Modell.Warenwirtschaft
{
    public sealed partial class ProduktProjektion
    {

        public static IEnumerable<Guid> AlleIDs(IEnumerable<Ereignis> history)
        {
            return history
                .OfType<Ereignis<ProduktWurdeEingelistet>>()
                .Select(_ => _.Daten.Produkt)
                .ToList();
        }

        private readonly Func<IEnumerable<Ereignis>> _history;

        public ProduktProjektion(Guid id, Func<IEnumerable<Ereignis>> history)
        {
            _history = history;
            Id = id;
        }



        public bool Eingelistet
        {
            get { return _history().OfType<Ereignis<ProduktWurdeEingelistet>>().Any(); }
        }



        public string Bezeichnung
        {
            get
            {
                return _history().OfType<Ereignis<ProduktWurdeEingelistet>>().Single().Daten.Bezeichnung;
            }
        }

        public readonly Guid Id;
    }
}
