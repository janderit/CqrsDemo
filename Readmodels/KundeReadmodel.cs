using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Modell.Kunden;
using Resourcen.Kunden;

namespace Readmodels
{
    public class KundeReadmodel
    {
        private readonly Func<Guid, IEnumerable<Ereignis>> _history;

        public KundeReadmodel(Func<Guid, IEnumerable<Ereignis>> history)
        {
            if (history == null) throw new ArgumentNullException("history");
            _history = history;
        }

        public KundeInfo Access(Guid kunde)
        {
            var history = _history(kunde).ToList();
            var projektor = new KundenProjektion(kunde, () => history);
            return new KundeInfo
                       {
                           Id = kunde,
                           Name = projektor.Name,
                           Anschrift = projektor.AktuelleAnschrift,
                           Warenkorb=projektor.Warenkorb
                       };
        }
    }
}
