using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;
using Modell.Shop;
using Resourcen.Shop;

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

        public WarenkorbInfo Access(Guid warenkorb)
        {
            var history = _history(warenkorb).ToList();
            var projektor = new WarenkorbProjektion(warenkorb, () => history);
            return new WarenkorbInfo
            {
                Id = warenkorb,
                Kunde = projektor.Kunde,
                Artikel = projektor.Artikel.ToList()
            };
        }
    }
}
