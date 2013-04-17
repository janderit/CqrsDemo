using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Warenwirtschaft;
using Resourcen.Bestellwesen;
using Kunde = Resourcen.Kunden.Kunde;
using Produkt = Resourcen.Warenwirtschaft.Produkt;

namespace Readmodels
{
    public class ProduktReadmodel
    {
        private readonly Func<Guid, IEnumerable<Ereignis>> _history;

        public ProduktReadmodel(Func<Guid, IEnumerable<Ereignis>> history)
        {
            if (history == null) throw new ArgumentNullException("history");
            _history = history;
        }

        public Produkt Access(Guid auftrag)
        {
            var history = _history(auftrag).ToList();
            var projektor = new ProduktProjektion(auftrag, () => history);
            return new Produkt
                       {
                           Id = auftrag,
                           Bezeichnung = projektor.Bezeichnung,
                           LagerBestand = projektor.Lagerbestand,
                           Verfuegbar= projektor.Verfuegbar,
                           Nachbestellt=projektor.Nachbestellt,
                           MengeImZulauf = projektor.MengeImZulauf,
                           AutomatischeNachbestellungen=projektor.AutomatischeNachbestellungen
                       };
        }
    }
}
