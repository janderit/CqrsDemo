using System;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Infrastruktur.Messaging;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Shop;
using Modell.Warenwirtschaft;
using Readmodels;

namespace Host
{

    public partial class CqrsHost : Port
    {

        private readonly EventStore _eventStore;
        private readonly KundeReadmodel _kunden;
        private readonly ProduktReadmodel _produkte;
        private readonly LagerbestandReadmodel _lagerbestand;
        private readonly AuftragReadmodel _auftraege;
        private readonly MetaReadmodel _meta;
        private readonly WarenkorbReadmodel _warenkoerbe;

        public CqrsHost(EventStore eventStore)
        {
            if (eventStore == null) throw new ArgumentNullException("eventStore");
            _eventStore = eventStore;

            _kunden = new KundeReadmodel(_eventStore.Stream(Kunde.AggregateEvents.Filter));
            _auftraege = new AuftragReadmodel(_eventStore.Stream(Auftrag.AggregateEvents.Filter), ()=>_eventStore.History);
            _produkte = new ProduktReadmodel(_eventStore.Stream(Produkt.AggregateEvents.Filter));
            _lagerbestand = new LagerbestandReadmodel(_eventStore.Stream(Lagerposten.AggregateEvents.Filter), _produkte.Access);
            _warenkoerbe = new WarenkorbReadmodel(_eventStore.Stream(Warenkorb.AggregateEvents.Filter));
            _meta = new MetaReadmodel(_eventStore.Subscribe);

        }

        public void Handle(CommandEnvelope commandEnvelope)
        {
            var unitOfWork = new UnitOfWork(_eventStore);
            Handle(commandEnvelope, (dynamic)commandEnvelope.Aktion, unitOfWork);
            unitOfWork.Commit();
        }

        public Resource<T> Retrieve<T>(QueryEnvelope queryEnvelope)
        {
            var resource = Handle(queryEnvelope, (dynamic)queryEnvelope.Abfrage);
            if (resource == null)
            {
                throw new InvalidOperationException(string.Format("Query Handler liefert kein Ergebnis! {0}",
                                                                  queryEnvelope.Abfrage.GetType().Name));
            }

            if (resource is Resource<T>)
            {
                return (Resource<T>) resource;
            }

            var t = (T)resource;
            if (t == null)
            {
                throw new InvalidOperationException(
                    string.Format("Query Handler liefert unerwartetes Ergebnis! {0} {1} statt {2}",
                                  queryEnvelope.Abfrage.GetType().Name, resource.GetType().Name, typeof (T).Name));
            }

            return new Resource<T> {Body = t};
        }

        private void Handle(CommandEnvelope commandEnvelope, object aktion, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException(string.Format("Kein Command Handler für {0} definiert", aktion.GetType().Name));
        }

        private object Handle(QueryEnvelope queryEnvelope, object abfrage)
        {
            throw new NotImplementedException(string.Format("Kein Query Handler für {0} definiert", abfrage.GetType().Name));
        }


    }


}
