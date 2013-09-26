using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Warenwirtschaft;
using Resourcen.Bestellwesen;

namespace Readmodels
{
    public class MetaReadmodel
    {

        public MetaReadmodel(Action<Action<Ereignis>> subscribe)
        {
            subscribe(OnEvent);
        }

        private void OnEvent(Ereignis ereignis)
        {
            Handle((dynamic) ereignis);
        }

        private void Handle(Ereignis e)
        {
            // intentionally left blank
        }

        private readonly Dictionary<Guid, string> _bezeichnungen = new Dictionary<Guid, string>();

        private void Handle(Ereignis<KundeWurdeErfasst> e)
        {
            _bezeichnungen.Add(e.Daten.Kunde, e.Daten.Name);
        }

        private void Handle(Ereignis<ProduktWurdeEingelistet> e)
        {
            _bezeichnungen.Add(e.Daten.Produkt, e.Daten.Bezeichnung);
        }

        public string Alias(Guid id)
        {
            return _bezeichnungen.ContainsKey(id) ? _bezeichnungen[id] : id.ToString();
        }

    }
}
