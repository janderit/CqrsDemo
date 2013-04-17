using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur;
using Infrastruktur.Common;
using Modell.Bestellwesen;

namespace Modell.Warenwirtschaft
{
    public sealed class ProduktProjektion
    {

        public static IEnumerable<Guid> AlleIDs(IEnumerable<Ereignis> history)
        {
            return history.OfType<Ereignis<ProduktWurdeEingelistet>>().Select(_ => _.EventSource).ToList();
        }

        private readonly Func<IEnumerable<Ereignis>> _history;

        public ProduktProjektion(Guid id, Func<IEnumerable<Ereignis>> history)
        {
            _history = history;
            Id = id;
        }



        public bool Eingelistet
        {
            get { return _history().OfType<Ereignis<ProduktWurdeEingelistet>>().Any(); }
        }



        public string Bezeichnung
        {
            get
            {
                return _history().OfType<Ereignis<ProduktWurdeEingelistet>>().Single().Daten.Bezeichnung;
            }
        }

        public int Lagerbestand
        {
            get
            {
                var bestand = 0;
                TypeSwitcher<Ereignis>.Create()
                                      .On<Ereignis<LieferungIstEingegangen>>(e => bestand += e.Daten.Menge)
                                      .On<Ereignis<WarenWurdenAusgeliefert>>(e => bestand -= e.Daten.Menge)
                                      .Run(_history());
                return bestand;

            }
        }

        public int Verfuegbar
        {
            get
            {
                var bestand = 0;
                TypeSwitcher<Ereignis>.Create()
                                      .On<Ereignis<NachbestellungWurdeBeauftragt>>(e => bestand += e.Daten.Menge)
                                      .On<Ereignis<WareWurdeDisponiert>>(e => bestand -= e.Daten.Menge)
                                      .Run(_history());
                return bestand;
            }
        }

        public int MengeImZulauf
        {
            get
            {
                var bestand = 0;
                TypeSwitcher<Ereignis>.Create()
                                      .On<Ereignis<NachbestellungWurdeBeauftragt>>(e => bestand += e.Daten.Menge)
                                      .On<Ereignis<LieferungIstEingegangen>>(e => bestand -= e.Daten.Menge)
                                      .Run(_history());
                return bestand;
            }
        }

        public bool Nachbestellt { get { return MengeImZulauf > 0; } }

        public int OffeneBestellungen
        {
            get
            {
                var bestand = 0;
                TypeSwitcher<Ereignis>.Create()
                                      .On<Ereignis<NachbestellungWurdeBeauftragt>>(e => bestand += e.Daten.Menge)
                                      .On<Ereignis<LieferungIstEingegangen>>(e => bestand -= e.Daten.Menge)
                                      .Run(_history());
                return bestand;
            }
        }

        public bool AutomatischeNachbestellungen
        {
            get
            {
                var aktiv = false;
                TypeSwitcher<Ereignis>.Create()
                                      .On<Ereignis<AutomatischeNachbestellungenWurdenAktiviert>>(e => aktiv = true)
                                      .On<Ereignis<AutomatischeNachbestellungenWurdenDeaktiviert>>(e => aktiv = false)
                                      .Run(_history());
                return aktiv;
            }
        }

        public int MindestVerfuegbarkeit
        {
            get
            {
                return _history().OfType<Ereignis<MindestVerfuegbarkeitWurdeDefiniert>>().Select(_ => (int?)_.Daten.Menge).LastOrDefault() ?? 0;
            }
        }

        public int MindestBestellmenge
        {
            get
            {
                return _history().OfType<Ereignis<MindestBestellmengeWurdeDefiniert>>().Select(_ => (int?)_.Daten.Menge).LastOrDefault() ?? 0;
            }
        }

        public readonly Guid Id;
    }
}
