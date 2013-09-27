using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Shop;
using Modell.Warenwirtschaft;
using Resourcen.Bestellwesen;
using Kunde = Resourcen.Kunden.Kunde;
using Produkt = Resourcen.Warenwirtschaft.Produkt;
using Warenkorb = Resourcen.Shop.Warenkorb;

namespace Readmodels
{
    public class WarenkorbReadmodel
    {
        private readonly Func<Guid, IEnumerable<Ereignis>> _history;

        public WarenkorbReadmodel(Func<Guid, IEnumerable<Ereignis>> history)
        {
            if (history == null) throw new ArgumentNullException("history");
            _history = history;
        }

        public Warenkorb Access(Guid warenkorb)
        {
            var history = _history(warenkorb).ToList();
            var projektor = new WarenkorbProjektion(warenkorb, () => history);
            return new Warenkorb
                {
                    Id = warenkorb,
                    Kunde = projektor.Kunde,
                    Artikel = projektor.Artikel.ToList()
                };
        }
    }
}
