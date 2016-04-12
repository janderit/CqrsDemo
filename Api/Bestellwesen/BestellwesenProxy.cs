using System;
using Api.Bestellwesen.Abfragen;
using Api.Bestellwesen.Aktionen;
using Infrastruktur.Messaging;
using Resourcen.Bestellwesen;

namespace Api.Bestellwesen
{
    public sealed class BestellwesenProxy : BestellwesenApi
    {
        private readonly Port _port;

        public BestellwesenProxy(Port port)
        {
            _port = port;
        }

        public Bestellungenliste OffeneBestellungen()
        {
            return _port.Retrieve<Bestellungenliste>(new QueryEnvelope(new OffeneBestellungenAbfrage())).Body;
        }

        public void AuftragErfassen(Guid auftrag, Guid kunde, Guid produkt, int menge)
        {
            _port.Handle(new CommandEnvelope(new AuftragErfassen(auftragsId: auftrag, kunde: kunde, menge: menge, produkt: produkt)));
        }

        public void AuftragAusfuehren(Guid auftrag)
        {
            _port.Handle(new CommandEnvelope(new AuftragAusfuehren(auftragId: auftrag)));
        }

    }
}