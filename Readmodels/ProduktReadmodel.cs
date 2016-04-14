using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;
using Modell.Warenwirtschaft;
using Resourcen.Warenwirtschaft;

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

        public ProduktInfo Access(Guid produkt)
        {
            var history = _history(produkt).ToList();
            var projektor = new ProduktProjektion(produkt, () => history);
            return new ProduktInfo
            {
                Id = produkt,
                Bezeichnung = projektor.Bezeichnung
            };
        }
    }
}
