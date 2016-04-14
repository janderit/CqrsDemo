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

        public ProduktlisteEx ProduktlisteEx()
        {
            return _port.Retrieve<ProduktlisteEx>(new QueryEnvelope(new ProduktlisteExAbfrage())).Body;
        }

        public Lagerbestandsliste Lagerbestandsliste(Guid lager)
        {
            return _port.Retrieve<Lagerbestandsliste>(new QueryEnvelope(new LagerbestandsAbfrage {LagerId = lager})).Body;
        }

        public void Einlisten(Guid produkt, string bezeichnung)
        {
            _port.Handle(new CommandEnvelope(new ProduktEinlisten(produktId: produkt, bezeichnung: bezeichnung)));
        }

        public void Nachbestellen(Guid lagerId, Guid produkt, int menge)
        {
            _port.Handle(new CommandEnvelope(
                new NachbestellungBeauftragen(lagerId: lagerId, produktId: produkt, bestellteMenge: menge)
                ));
        }

        public void WareneingangVerzeichnen(Guid lagerId, Guid produkt)
        {
            _port.Handle(new CommandEnvelope(new WareneingangVerbuchen(lagerId: lagerId, produktId: produkt)));
        }

        public ProduktInfo ProduktAbrufen(Guid produkt)
        {
            return Produktliste().Produkte.SingleOrDefault(_ => _.Id == produkt);
        }

        public ProduktInfoEx ProduktExAbrufen(Guid produkt)
        {
            return ProduktlisteEx().Produkte.SingleOrDefault(_ => _.Id == produkt);
        }

        public Lagerbestandsliste LagerbestandslisteAbrufen(Guid lager)
        {
            return Lagerbestandsliste(lager);
        }

        public LagerbestandInfo LagerbestandAbrufen(Guid lager, Guid produkt)
        {
            return Lagerbestandsliste(lager).Bestand.SingleOrDefault(_ => _.Produkt == produkt) ?? new LagerbestandInfo {AutomatischeNachbestellungen=false, Lager=lager, LagerBestand=0, MengeImZulauf=0, Nachbestellt=false, Produkt=produkt, Produktbezeichnung="?"};
        }

        public void MindestVerfuegbarkeitDefinieren(Guid lagerId, Guid produkt, int mindestverfuegbarkeit, int mindestbestellmenge)
        {
            _port.Handle(new CommandEnvelope(new MindestVerfuegbarkeitDefinieren(lagerId: lagerId, produktId: produkt, mindestVerfuegbarkeit: mindestverfuegbarkeit, mindestBestellmenge: mindestbestellmenge)));
        }

        public void AutomatischeNachbestellungenDeaktivieren(Guid lagerId, Guid produkt)
        {
            _port.Handle(new CommandEnvelope(new AutomatischeNachbestellungenDeaktivieren(lagerId: lagerId, produktId: produkt)));
        }
    }
}

