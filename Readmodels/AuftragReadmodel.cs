using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Modell.Bestellwesen;
using Modell.Kunden;
using Resourcen.Bestellwesen;
using Kunde = Resourcen.Kunden.Kunde;

namespace Readmodels
{
    public class AuftragReadmodel
    {
        private readonly Func<Guid, IEnumerable<Ereignis>> _history;

        public AuftragReadmodel(Func<Guid, IEnumerable<Ereignis>> history)
        {
            if (history == null) throw new ArgumentNullException("history");
            _history = history;
        }

        public Bestellung Access(Guid auftrag)
        {
            var history = _history(auftrag).ToList();
            var projektor = new AuftragProjektion(() => history);
            return new Bestellung
                       {
                           Id = auftrag,
                           Kunde = projektor.Kunde,
                           Menge = projektor.Menge,
                           Produkt=projektor.Produkt,
                           Erfuellt=projektor.Erfuellt,
                       };
        }
    }
}
