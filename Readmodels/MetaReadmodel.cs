using System;
using System.Collections.Generic;
using Infrastruktur.Common;
using Modell.Kunden;
using Modell.Shop;
using Modell.Warenwirtschaft;

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

        private readonly Dictionary<Guid, Func<string>> _bezeichnungen = new Dictionary<Guid, Func<string>>();

        private void Handle(Ereignis<KundeWurdeErfasst> e)
        {
            _bezeichnungen.Add(e.Daten.Kunde, ()=>e.Daten.Name);
        }

        private void Handle(Ereignis<WarenkorbWurdeEroeffnet> e)
        {
            _bezeichnungen.Add(e.Daten.Warenkorb, () => "für " + Alias(e.Daten.Kunde));
        }

        private void Handle(Ereignis<ProduktWurdeEingelistet> e)
        {
            _bezeichnungen.Add(e.Daten.Produkt, ()=>e.Daten.Bezeichnung);
        }

        public string Alias(Guid id)
        {
            return _bezeichnungen.ContainsKey(id) ? _bezeichnungen[id]() : id.ToString();
        }

    }
}
