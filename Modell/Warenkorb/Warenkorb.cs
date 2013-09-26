using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;

namespace Modell.Warenkorb
{

    public class Warenkorb : AggregateRoot
    {
        private readonly WarenkorbProjektion _zustand;

        public Warenkorb(WarenkorbProjektion zustand, Action<Ereignis> eventsink)
            : base(eventsink)
        {
            _zustand = zustand;
        }

        public Guid Id
        {
            get { return _zustand.Id; }
        }

        public Guid Kunde
        {
            get { return _zustand.Kunde; }
        }


        public void Eroeffnen(Guid id, Guid kunde)
        {
            WurdeEroffnet(id, kunde);
        }

        public void FuegeHinzu(Guid produkt, int menge)
        {
            ArtikelWurdeHinzugefuegt(produkt, menge);
        }



        private void WurdeEroffnet(Guid warenkorb, Guid kunde)
        {
            Publish(new WarenkorbWurdeEroeffnet() {Warenkorb = warenkorb, Kunde = kunde});
        }

        private void ArtikelWurdeHinzugefuegt(Guid produkt, int menge)
        {
            Publish(new ArtikelWurdeZuWarenkorbHinzugefuegt {Warenkorb = Id, Produkt = produkt, Menge = menge});
        }

        
    }
}


