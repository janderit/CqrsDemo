using System;
using System.Linq;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Resourcen.Shop;

namespace Modell.Shop
{

    public class Warenkorb : AggregateRoot
    {
        private readonly WarenkorbProjektion _zustand;

        public static readonly AggregateEvents AggregateEvents = new AggregateEvents()
            .AggregateIsAffectedBy<WarenkorbWurdeEroeffnet>(e => e.Warenkorb)
            .AggregateIsAffectedBy<ArtikelWurdeZuWarenkorbHinzugefuegt>(e => e.Warenkorb)
            .AggregateIsAffectedBy<ArtikelWurdeAusWarenkorbEntfernt>(e => e.Warenkorb);

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

        public void Entfernen(Guid zeile)
        {
            var zu_entfernen = _zustand.Artikel.SingleOrDefault(_ => _.ZeileId == zeile);
            if (zu_entfernen!=null) ArtikelWurdeEntfernt(zu_entfernen);
        }

        public void Leeren()
        {
            _zustand.Artikel.Select(art => art.ZeileId).ToList().ForEach(Entfernen);
        }



        private void WurdeEroffnet(Guid kunde)
        {
            Publish(new WarenkorbWurdeEroeffnet() {Warenkorb = Id, Kunde = kunde});
        }

        private void ArtikelWurdeHinzugefuegt(Guid produkt, int menge)
        {
            Publish(new ArtikelWurdeZuWarenkorbHinzugefuegt() {Zeile = Guid.NewGuid(), Warenkorb = Id, Produkt = produkt, Menge = menge});
        }

        private void ArtikelWurdeEntfernt(ArtikelImWarenkorb zeile)
        {
            Publish(new ArtikelWurdeAusWarenkorbEntfernt
                {
                    Warenkorb = Id,
                    Produkt = zeile.Produkt,
                    Menge = zeile.Menge,
                    Zeile = zeile.ZeileId
                });
        }


        public void Bestellen(Action<Guid, int, Guid> auftrag_factory)
        {
            while (_zustand.Artikel.Any())
            {
                var artikel = _zustand.Artikel.First();
                auftrag_factory(artikel.Produkt, artikel.Menge, _zustand.Kunde);
                Entfernen(artikel.ZeileId);
            }
        }
    }
}


