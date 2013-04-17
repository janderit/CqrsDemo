using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Bestellwesen.Abfragen;
using Api.Bestellwesen.Aktionen;
using Api.Kunden.Abfragen;
using Api.Kunden.Aktionen;
using Infrastruktur;
using Infrastruktur.Messaging;
using Resourcen.Bestellwesen;
using Resourcen.Kunden;

namespace Api.Bestellwesen
{
    public sealed class BestellwesenApi
    {
        private readonly Port _port;

        public BestellwesenApi(Port port)
        {
            _port = port;
        }

        public Bestellungsliste OffeneBestellungen()
        {
            return _port.Retrieve<Bestellungsliste>(new Query() { Abfrage = new OffeneBestellungenAbfrage() }).Body;
        }

        public Guid AuftragErfassen(Guid kunde, int menge, Guid produkt)
        {
            var id = Guid.NewGuid();
            _port.Handle(new Command { Aktion = new AuftragErfassen() { AuftragsId=id, Kunde=kunde, Menge=menge, Produkt=produkt } });
            return id;
        }

        public void AuftragAusfuehren(Guid id)
        {
            _port.Handle(new Command { Aktion = new AuftragAusfuehren() { AuftragId=id } });
        }

    }
}
