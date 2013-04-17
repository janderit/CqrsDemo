using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Kunden.Abfragen;
using Api.Kunden.Aktionen;
using Api.Warenwirtschaft.Abfragen;
using Api.Warenwirtschaft.Aktionen;
using Infrastruktur;
using Infrastruktur.Messaging;
using Resourcen.Kunden;
using Resourcen.Warenwirtschaft;

namespace Api.Warenwirtschaft
{
    public sealed class WarenwirtschaftApi
    {
        private readonly Port _port;

        public WarenwirtschaftApi(Port port)
        {
            _port = port;
        }

        public Produktliste Produktliste()
        {
            return _port.Retrieve<Produktliste>(new Query() { Abfrage = new ProduktlisteAbfrage() }).Body;
        }


        public Guid Einlisten(string bezeichnung)
        {
            var id = Guid.NewGuid();
            _port.Handle(new Command { Aktion = new ProduktEinlisten() { ProduktId=id, Bezeichnung = bezeichnung} });
            return id;
        }

        public void Nachbestellen(Guid id, int menge)
        {
            _port.Handle(new Command { Aktion = new NachbestellungBeauftragen() { ProduktId = id, BestellteMenge=menge } });
        }

        public void Wareneingang(Guid id)
        {
            _port.Handle(new Command { Aktion = new WareneingangVerbuchen() { ProduktId = id } });
        }

        public Produkt Produkt(Guid id)
        {
            return Produktliste().Produkte.SingleOrDefault(_ => _.Id == id);
        }

        public void MindestVerfuegbarkeitDefinieren(Guid produkt, int mindestverfuegbarkeit, int mindestbestellmenge)
        {
            _port.Handle(new Command { Aktion = new MindestVerfuegbarkeitDefinieren() { ProduktId = produkt, MindestVerfuegbarkeit = mindestverfuegbarkeit, MindestBestellmenge = mindestbestellmenge } });
        }

        public void AutomatischeNachbestellungenDeaktivieren(Guid produkt)
        {
            _port.Handle(new Command { Aktion = new AutomatischeNachbestellungenDeaktivieren() { ProduktId = produkt} });
        }
    }
}
