using System;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;

namespace Modell.Warenwirtschaft
{
    public class Lagerposten : AggregateRoot
    {
        private readonly Guid _lager;
        public readonly Guid Produkt;
        private readonly LagerProjektion _zustand;

        public static AggregateEvents AggregateEvents = new AggregateEvents()
            .AggregateIsAffectedBy<NachbestellungWurdeBeauftragt>(e => e.Lager)
            .AggregateIsAffectedBy<LieferungIstEingegangen>(e => e.Lager)
            .AggregateIsAffectedBy<WarenWurdenAusgeliefert>(e => e.Lager)
            .AggregateIsAffectedBy<AutomatischeNachbestellungenWurdenAktiviert>(e => e.Lager)
            .AggregateIsAffectedBy<AutomatischeNachbestellungenWurdenDeaktiviert>(e => e.Lager)
            .AggregateIsAffectedBy<MindestBestellmengeWurdeDefiniert>(e => e.Lager)
            .AggregateIsAffectedBy<MindestVerfuegbarkeitWurdeDefiniert>(e => e.Lager);


        public Lagerposten(Guid lager, Guid produkt, LagerProjektion zustand, Action<Ereignis> eventsink) : base(eventsink)
        {
            _lager = lager;
            Produkt = produkt;
            _zustand = zustand;
        }

        public void Nachbestellen(int menge)
        {
            if (menge < 1) throw new VorgangNichtAusgefuehrt("Bestellmenge muss >=1 sein.");
            NachbestellungWurdeBeauftragt(menge);
        }


        public void Wareneingang()
        {
            if (!_zustand.Nachbestellt(Produkt)) throw new VorgangNichtAusgefuehrt("Einbuchung eines Wareneingangs ohne vorherige Nachbestellung ist nicht möglich");
            LieferungIstEingegangen(_zustand.OffeneBestellungen(Produkt));
            PruefeAutomatischeNachbestellung();
        }

        internal bool AuftragsannahmePruefen(int menge)
        {
            return menge <= _zustand.Verfuegbar(Produkt);
        }

        public bool BestandPruefen(int menge)
        {
            return menge <= _zustand.Lagerbestand(Produkt);
        }

        internal void Ausliefern(int menge)
        {
            WarenWurdenAusgeliefert(menge);
            PruefeAutomatischeNachbestellung();
        }

        public void MindestVerfuegbarkeitDefinieren(int mindestVerfuegbarkeit, int mindestBestellmenge)
        {
            MindestVerfuegbarkeitWurdeDefiniert(mindestVerfuegbarkeit);
            MindestBestellmengeWurdeDefiniert(mindestBestellmenge);
            if (!_zustand.AutomatischeNachbestellungen(Produkt)) AutomatischeNachbestellungenWurdenAktiviert();

            PruefeAutomatischeNachbestellung();
        }

        private void PruefeAutomatischeNachbestellung()
        {
            if (_zustand.AutomatischeNachbestellungen(Produkt) && _zustand.Verfuegbar(Produkt) < _zustand.MindestVerfuegbarkeit(Produkt))
            {
                if (_zustand.Nachbestellt(Produkt)) return; // Technische Limitierung: die aktuelle Implementierung unterstützt nur eine aktive Nachbestellung!

                var delta = _zustand.MindestVerfuegbarkeit(Produkt) - _zustand.Verfuegbar(Produkt);
                if (delta < _zustand.MindestBestellmenge(Produkt)) delta = _zustand.MindestBestellmenge(Produkt);

                if (delta > 0) Nachbestellen(delta);
            }
        }


        public void AutomatischeNachbestellungenDeaktivieren()
        {
            if (!_zustand.AutomatischeNachbestellungen(Produkt)) return;
            AutomatischeNachbestellungenWurdenDeaktiviert();
        }


#region Event factory methods
        private void AutomatischeNachbestellungenWurdenDeaktiviert()
        {
            Publish(new AutomatischeNachbestellungenWurdenDeaktiviert {Lager = _lager, Produkt = Produkt});
        }

        private void AutomatischeNachbestellungenWurdenAktiviert()
        {
            Publish(new AutomatischeNachbestellungenWurdenAktiviert { Lager = _lager, Produkt = Produkt });
        }

        private void MindestBestellmengeWurdeDefiniert(int menge)
        {
            Publish(new MindestBestellmengeWurdeDefiniert() { Lager = _lager, Produkt = Produkt, Menge = menge });
        }

        private void MindestVerfuegbarkeitWurdeDefiniert(int menge)
        {
            Publish(new MindestVerfuegbarkeitWurdeDefiniert() { Lager = _lager, Produkt = Produkt, Menge = menge });
        }

        private void NachbestellungWurdeBeauftragt(int menge)
        {
            Publish(new NachbestellungWurdeBeauftragt() { Lager = _lager, Produkt = Produkt, Menge = menge });
        }

        private void LieferungIstEingegangen(int menge)
        {
            Publish(new LieferungIstEingegangen() { Lager = _lager, Produkt = Produkt, Menge = menge });
        }

        private void WarenWurdenAusgeliefert(int menge)
        {
            Publish(new WarenWurdenAusgeliefert() { Lager = _lager, Produkt = Produkt, Menge = menge });
        }
#endregion

    }
}


