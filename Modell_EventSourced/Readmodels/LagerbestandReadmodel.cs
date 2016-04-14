using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;
using Modell.Warenwirtschaft;
using Modell_shared;
using Resourcen.Warenwirtschaft;

namespace Readmodels
{
    public class LagerbestandReadmodel
    {
        private readonly Func<Guid, IEnumerable<Ereignis>> _history;
        private readonly Func<Guid, ProduktInfo> _joinProdukte;

        public LagerbestandReadmodel(Func<Guid, IEnumerable<Ereignis>> history, Func<Guid, ProduktInfo> join_produkte)
        {
            if (history == null) throw new ArgumentNullException("history");
            _history = history;
            _joinProdukte = join_produkte;
        }

        public LagerbestandInfo Access(Guid lager, Guid produkt)
        {
            var history = _history(lager).ToList();
            var projektor = new LagerProjektion(() => history);
            return LagerbestandInfo(lager, produkt, projektor);
        }

        private LagerbestandInfo LagerbestandInfo(Guid lager, Guid produkt, LagerProjektion projektor)
        {
            return new LagerbestandInfo()
            {
                Lager = lager,
                Produkt = produkt,
                Produktbezeichnung = _joinProdukte(produkt).Bezeichnung,
                LagerBestand = projektor.Lagerbestand(produkt),
                Nachbestellt = projektor.Nachbestellt(produkt),
                MengeImZulauf = projektor.MengeImZulauf(produkt),
                AutomatischeNachbestellungen = projektor.AutomatischeNachbestellungen(produkt)
            };
        }

        public List<LagerbestandInfo> Alle(Guid lager, List<Guid> produktIDs)
        {
            var history = _history(lager).ToList();
            var projektor = new LagerProjektion(() => history);
            return produktIDs.Select(produkt => LagerbestandInfo(lager, produkt, projektor)).ToList();
        }

        public int Verfuegbar_fuer(Guid produkt)
        {
            return Lagerliste.Alle.Select(lager => new LagerProjektion(() => _history(lager.Id).ToList()).Verfuegbar(produkt)).Sum();
        }

        public int LagerBestand_fuer(Guid produkt)
        {
            return Lagerliste.Alle.Select(lager => new LagerProjektion(() => _history(lager.Id).ToList()).Lagerbestand(produkt)).Sum();
        }
    }
}
