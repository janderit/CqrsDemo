using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur;
using Infrastruktur.Common;

namespace Modell.Warenwirtschaft
{
    public sealed class LagerProjektion
    {

        private readonly Func<IEnumerable<Ereignis>> _history;

        public LagerProjektion(Func<IEnumerable<Ereignis>> history)
        {
            _history = history;
        }

        public int Lagerbestand(Guid produkt)
        {
            var bestand = 0;
            TypeSwitcher<Ereignis>.Create()
                .On<Ereignis<LieferungIstEingegangen>>(e => e.Daten.Produkt == produkt, e => bestand += e.Daten.Menge)
                .On<Ereignis<WarenWurdenAusgeliefert>>(e => e.Daten.Produkt == produkt, e => bestand -= e.Daten.Menge)
                .Run(_history());
            return bestand;
        }

        public int Verfuegbar(Guid produkt)
        {
            var bestand = 0;
            TypeSwitcher<Ereignis>.Create()
                .On<Ereignis<NachbestellungWurdeBeauftragt>>(e => e.Daten.Produkt == produkt, e => bestand += e.Daten.Menge)
                .On<Ereignis<WarenWurdenAusgeliefert>>(e => e.Daten.Produkt == produkt, e => bestand -= e.Daten.Menge)
                .Run(_history());
            return bestand;
        }

        public int MengeImZulauf(Guid produkt)
        {
            var bestand = 0;
            TypeSwitcher<Ereignis>.Create()
                .On<Ereignis<NachbestellungWurdeBeauftragt>>(e => e.Daten.Produkt == produkt, e => bestand += e.Daten.Menge)
                .On<Ereignis<LieferungIstEingegangen>>(e => e.Daten.Produkt == produkt, e => bestand -= e.Daten.Menge)
                .Run(_history());
            return bestand;
        }

        public bool Nachbestellt(Guid produkt)
        {
            return MengeImZulauf(produkt) > 0;
        }

        public int OffeneBestellungen(Guid produkt)
        {
            var bestand = 0;
            TypeSwitcher<Ereignis>.Create()
                .On<Ereignis<NachbestellungWurdeBeauftragt>>(e => e.Daten.Produkt == produkt, e => bestand += e.Daten.Menge)
                .On<Ereignis<LieferungIstEingegangen>>(e => e.Daten.Produkt == produkt, e => bestand -= e.Daten.Menge)
                .Run(_history());
            return bestand;
        }

        public bool AutomatischeNachbestellungen(Guid produkt)
        {
                var aktiv = false;
                TypeSwitcher<Ereignis>.Create()
                    .On<Ereignis<AutomatischeNachbestellungenWurdenAktiviert>>(e => e.Daten.Produkt == produkt, e => aktiv = true)
                    .On<Ereignis<AutomatischeNachbestellungenWurdenDeaktiviert>>(e => e.Daten.Produkt == produkt, e => aktiv = false)
                    .Run(_history());
                return aktiv;
        }

        public int MindestVerfuegbarkeit(Guid produkt)
        {
                return _history().OfType<Ereignis<MindestVerfuegbarkeitWurdeDefiniert>>()
                    .Where(_ => _.Daten.Produkt == produkt)
                    .Select(_ => (int?) _.Daten.Menge).LastOrDefault() ?? 0;
        }

        public int MindestBestellmenge(Guid produkt)
        {
                return _history().OfType<Ereignis<MindestBestellmengeWurdeDefiniert>>()
                    .Where(_ => _.Daten.Produkt == produkt)
                    .Select(_ => (int?) _.Daten.Menge).LastOrDefault() ?? 0;
        }

        public List<Guid> AllePosten()
        {
            return _history().OfType<Ereignis<NachbestellungWurdeBeauftragt>>().Select(_ => _.Daten.Produkt).Distinct().ToList();
        }

    }
}
