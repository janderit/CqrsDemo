using System;
using System.Linq;
using Api.Warenwirtschaft.Abfragen;
using Api.Warenwirtschaft.Aktionen;
using Infrastruktur.Messaging;
using Resourcen.Warenwirtschaft;

namespace Api.Warenwirtschaft
{
    public sealed class WarenwirtschaftProxy : WarenwirtschaftApi
    {
        private readonly Port _port;

        public WarenwirtschaftProxy(Port port)
        {
            _port = port;
        }

        public Produktliste Produktliste()
        {
            return _port.Retrieve<Produktliste>(new QueryEnvelope(new ProduktlisteAbfrage())).Body;
        }

        public void Einlisten(Guid produkt, string bezeichnung)
        {
            _port.Handle(new CommandEnvelope(new ProduktEinlisten(produktId: produkt, bezeichnung: bezeichnung)));
        }

        public void Nachbestellen(Guid produkt, int menge)
        {
            _port.Handle(new CommandEnvelope(new NachbestellungBeauftragen(produktId: produkt, bestellteMenge: menge)));
        }

        public void WareneingangVerzeichnen(Guid produkt)
        {
            _port.Handle(new CommandEnvelope(new WareneingangVerbuchen(produktId: produkt)));
        }

        public ProduktInfo ProduktAbrufen(Guid produkt)
        {
            return Produktliste().Produkte.SingleOrDefault(_ => _.Id == produkt);
        }

        public void MindestVerfuegbarkeitDefinieren(Guid produkt, int mindestverfuegbarkeit, int mindestbestellmenge)
        {
            _port.Handle(new CommandEnvelope(new MindestVerfuegbarkeitDefinieren(produktId: produkt, mindestVerfuegbarkeit: mindestverfuegbarkeit, mindestBestellmenge: mindestbestellmenge)));
        }

        public void AutomatischeNachbestellungenDeaktivieren(Guid produkt)
        {
            _port.Handle(new CommandEnvelope(new AutomatischeNachbestellungenDeaktivieren(produktId: produkt)));
        }
    }
}