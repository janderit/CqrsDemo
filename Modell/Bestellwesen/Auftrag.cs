using System;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Modell.Kunden;
using Modell.Warenwirtschaft;

namespace Modell.Bestellwesen
{

    public class Auftrag : AggregateRoot
    {
        private readonly AuftragProjektion _zustand;

        public Auftrag(AuftragProjektion zustand, Action<Ereignis> eventsink):base(eventsink)
        {
            _zustand = zustand;
        }

        public Guid Produkt
        {
            get { return _zustand.Produkt; }
        }


        public void Erfassen(Produkt produkt, int menge, Kunde kunde)
        {
            if (_zustand.Erfasst) return;
            if (menge<1) throw new VorgangNichtAusgefuehrt("Die Bestellmenge muß > 0 sein");

            kunde.AuftragsannahmePruefen();
            if (!produkt.AuftragsannahmePruefen(menge)) throw new VorgangNichtAusgefuehrt("Die Bestellung überschreitet den verfügbaren Bestand.");

            produkt.FuerAuftragReservieren(menge);

            WurdeErfasst(produkt.Id, menge, kunde.Id);
        }

        public void Ausfuehren(Produkt produkt)
        {
            if (!_zustand.Erfasst) throw new NichtGefunden("Auftrag");
            if (_zustand.Erfuellt) return;

            if (!produkt.BestandPruefen(_zustand.Menge)) throw new VorgangNichtAusgefuehrt("Die Bestellung überschreitet den aktuellen Lagerbestand.");

            produkt.Ausliefern(_zustand.Menge);

            WurdeAusgefuehrt(_zustand.Produkt, _zustand.Menge);
        }



        private void WurdeErfasst(Guid produkt, int menge, Guid kunde)
        {
            Publish(new AuftragWurdeErfasst{Kunde=kunde, Produkt=produkt, Menge=menge});
        }

        private void WurdeAusgefuehrt(Guid produkt, int menge)
        {
            Publish(new AuftragWurdeErfuellt { Produkt=produkt, Menge=menge });
        }


        
    }
}


