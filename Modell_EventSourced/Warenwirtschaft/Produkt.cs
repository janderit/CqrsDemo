using System;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Modell.Bestellwesen;

namespace Modell.Warenwirtschaft
{

    public class Produkt : AggregateRoot
    {
        private readonly ProduktProjektion _zustand;

        public static AggregateEvents AggregateEvents = new AggregateEvents()
            .AggregateIsAffectedBy<ProduktWurdeEingelistet>(e => e.Produkt);


        public Produkt(ProduktProjektion zustand, Action<Ereignis> eventsink) : base(eventsink)
        {
            _zustand = zustand;
        }

        public Guid Id
        {
            get { return _zustand.Id; }
        }


        public void Einlisten(string bezeichnung)
        {
            if (_zustand.Eingelistet) return;
            WurdeEingelistet(bezeichnung);
        }


        private void WurdeEingelistet(string bezeichnung)
        {
            Publish(new ProduktWurdeEingelistet() { Produkt = Id,Bezeichnung = bezeichnung });
        }

    }
}


