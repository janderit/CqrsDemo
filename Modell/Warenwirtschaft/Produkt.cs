using System;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Modell.Bestellwesen;

namespace Modell.Warenwirtschaft
{

    public class Produkt : AggregateRoot
    {
        private readonly ProduktProjektion _zustand;

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

        public void Nachbestellen(int menge)
        {
            if (!_zustand.Eingelistet) throw new NichtGefunden("Produkt");
            if (menge < 1) throw new VorgangNichtAusgefuehrt("Bestellmenge muss >=1 sein.");
            NachbestellungWurdeBeauftragt(menge);
        }


        public void Wareneingang()
        {
            if (!_zustand.Eingelistet) throw new NichtGefunden("Produkt");
            if (!_zustand.Nachbestellt) throw new VorgangNichtAusgefuehrt("Einbuchung eines Wareneingangs ohne vorherige Nachbestellung ist nicht möglich");
            LieferungIstEingegangen(_zustand.OffeneBestellungen);
            PruefeAutomatischeNachbestellung();
        }

        internal bool AuftragsannahmePruefen(int menge)
        {
            if (!_zustand.Eingelistet) throw new NichtGefunden("Produkt");
            return menge <= _zustand.Verfuegbar;
        }

        public bool BestandPruefen(int menge)
        {
            if (!_zustand.Eingelistet) throw new NichtGefunden("Produkt");
            return menge <= _zustand.Lagerbestand;
        }

        internal void FuerAuftragReservieren(int menge)
        {
            WareWurdeDisponiert(menge);
            PruefeAutomatischeNachbestellung();
        }

        internal void Ausliefern(int menge)
        {
            WarenWurdenAusgeliefert(menge);
        }

        public void MindestVerfuegbarkeitDefinieren(int mindestVerfuegbarkeit, int mindestBestellmenge)
        {
            MindestVerfuegbarkeitWurdeDefiniert(mindestVerfuegbarkeit);
            MindestBestellmengeWurdeDefiniert(mindestBestellmenge);
            if (!_zustand.AutomatischeNachbestellungen) AutomatischeNachbestellungenWurdenAktiviert();

            PruefeAutomatischeNachbestellung();
        }

        private void PruefeAutomatischeNachbestellung()
        {
            if (_zustand.AutomatischeNachbestellungen && _zustand.Verfuegbar < _zustand.MindestVerfuegbarkeit)
            {
                if (_zustand.Nachbestellt) return; // Technische Limitierung: die aktuelle Implementierung unterstützt nur eine aktive Nachbestellung!

                var delta = _zustand.MindestVerfuegbarkeit - _zustand.Verfuegbar;
                if (delta < _zustand.MindestBestellmenge) delta = _zustand.MindestBestellmenge;

                if (delta > 0) Nachbestellen(delta);
            }
        }


        public void AutomatischeNachbestellungenDeaktivieren()
        {
            if (!_zustand.AutomatischeNachbestellungen) return;
            AutomatischeNachbestellungenWurdenDeaktiviert();
        }

        private void AutomatischeNachbestellungenWurdenDeaktiviert()
        {
            Publish(new AutomatischeNachbestellungenWurdenDeaktiviert());
        }


        private void AutomatischeNachbestellungenWurdenAktiviert()
        {
            Publish(new AutomatischeNachbestellungenWurdenAktiviert() { });
        }
        
        private void MindestBestellmengeWurdeDefiniert(int menge)
        {
            Publish(new MindestBestellmengeWurdeDefiniert() { Menge = menge });
        }

        private void MindestVerfuegbarkeitWurdeDefiniert(int menge)
        {
            Publish(new MindestVerfuegbarkeitWurdeDefiniert() { Menge = menge });
        }

        private void WurdeEingelistet(string bezeichnung)
        {
            Publish(new ProduktWurdeEingelistet(){Bezeichnung = bezeichnung});
        }

        private void NachbestellungWurdeBeauftragt(int menge)
        {
            Publish(new NachbestellungWurdeBeauftragt() { Menge = menge });
        }

        private void LieferungIstEingegangen(int menge)
        {
            Publish(new LieferungIstEingegangen() { Menge = menge });
        }

        private void WareWurdeDisponiert(int menge)
        {
            Publish(new WareWurdeDisponiert() { Menge = menge });
        }

        private void WarenWurdenAusgeliefert(int menge)
        {
            Publish(new WarenWurdenAusgeliefert() { Menge = menge });
        }

        
    }
}


