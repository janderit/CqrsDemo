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

        public static readonly AggregateEvents AggregateEvents = new AggregateEvents()
            .AggregateIsAffectedBy<WarenkorbWurdeEroeffnet>(e => e.Warenkorb)
            .AggregateIsAffectedBy<ArtikelWurdeZuWarenkorbHinzugefuegt>(e => e.Warenkorb);

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


        public void Eroeffnen(Guid kunde)
        {
            WurdeEroffnet(kunde);
        }

        public void FuegeHinzu(Guid produkt, int menge)
        {
            ArtikelWurdeHinzugefuegt(produkt, menge);
        }



        private void WurdeEroffnet(Guid kunde)
        {
            Publish(new WarenkorbWurdeEroeffnet() {Warenkorb = Id, Kunde = kunde});
        }

        private void ArtikelWurdeHinzugefuegt(Guid produkt, int menge)
        {
            Publish(new ArtikelWurdeZuWarenkorbHinzugefuegt {Warenkorb = Id, Produkt = produkt, Menge = menge});
        }

        
    }
}


