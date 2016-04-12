using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;
using Modell.Bestellwesen;
using Resourcen.Bestellwesen;

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

        public BestellungInfo Access(Guid auftrag)
        {
            var history = _history(auftrag).ToList();
            var projektor = new AuftragProjektion(() => history);
            return new BestellungInfo
            {
                Id = auftrag,
                Kunde = projektor.Kunde,
                Menge = projektor.Menge,
                Produkt = projektor.Produkt,
                Erfuellt = projektor.Erfuellt,
            };
        }
    }
}
