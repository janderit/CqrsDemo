using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Modell.Shop;

namespace Modell.Kunden
{

    public partial class Kunde : AggregateRoot
    {
        private readonly KundenProjektion _zustand;

        public static AggregateEvents AggregateEvents = new AggregateEvents()
            .AggregateIsAffectedBy<KundeWurdeErfasst>(e => e.Kunde)
            .AggregateIsAffectedBy<AnschriftWurdeGeaendert>(e => e.Kunde)
            .AggregateIsAffectedBy<WarenkorbWurdeEroeffnet>(e => e.Kunde);

        public Kunde(KundenProjektion zustand, Action<Ereignis> eventsink):base(eventsink)
        {
            _zustand = zustand;
        }

        public Guid Id
        {
            get { return _zustand.Id; }
        }


        public void Erfassen(string name, string anschrift, Warenkorb warenkorb)
        {
            if (_zustand.IstErfasst) return;
            WurdeErfasst(name, anschrift);
            warenkorb.Eroeffnen(Id);
        }

        public void AuftragsannahmePruefen()
        {
            if (!_zustand.IstErfasst) throw new NichtGefunden("Kunde");
        }

        public void AnschriftAendern(string neueanschrift)
        {
            if (!_zustand.IstErfasst) throw new NichtGefunden("Kunde");
            if (_zustand.AktuelleAnschrift == neueanschrift) return;
            AnschriftWurdeGeaendert(neueanschrift);
        }

        private void WurdeErfasst(string name, string anschrift)
        {
            Publish(new KundeWurdeErfasst() {Kunde = Id, Name = name, Anschrift = anschrift});
        }

        private void AnschriftWurdeGeaendert(string neueanschrift)
        {
            Publish(new AnschriftWurdeGeaendert()
                {
                    Kunde = Id,
                    NeueAnschrift = neueanschrift
                });
        }

    }
}


