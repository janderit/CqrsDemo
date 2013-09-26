using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Modell.Warenkorb;

namespace Modell.Kunden
{
    public sealed class KundenProjektion
    {

        public static IEnumerable<Guid> AlleIDs(IEnumerable<Ereignis> history)
        {
            return history.OfType<Ereignis<KundeWurdeErfasst>>().Select(_ => _.Daten.Kunde).ToList();
        }

        private readonly Func<IEnumerable<Ereignis>> _history;

        public KundenProjektion(Guid id, Func<IEnumerable<Ereignis>> history)
        {
            _history = history;
            Id = id;
        }

        public readonly Guid Id;

        public bool IstErfasst {get { return _history().OfType<Ereignis<KundeWurdeErfasst>>().Any(); }}

        public string Name
        {
            get
            {
                return _history().OfType<Ereignis<KundeWurdeErfasst>>().Single().Daten.Name;
            }
        }

        public Guid Warenkorb
        {
            get
            {
                return _history().OfType<Ereignis<WarenkorbWurdeEroeffnet>>().Last().Daten.Warenkorb;
            }
        }

        public string AktuelleAnschrift {get
        {
            var letzteAenderung = _history().OfType<Ereignis<AnschriftWurdeGeaendert>>()
                .LastOrDefault();

            if (letzteAenderung==null) return _history()
                .OfType<Ereignis<KundeWurdeErfasst>>().Single().Daten.Anschrift;

            return letzteAenderung.Daten.NeueAnschrift;
        }}

    }
}
