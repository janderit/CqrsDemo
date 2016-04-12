using System;
using System.Linq;
using Api.Kunden.Abfragen;
using Api.Kunden.Aktionen;
using Infrastruktur.Messaging;
using Resourcen.Kunden;

namespace Api.Kunden
{
    public sealed class KundenProxy : KundenApi
    {
        private readonly Port _port;

        public KundenProxy(Port port)
        {
            _port = port;
        }

        public Kundenliste Kundenliste()
        {
            return _port.Retrieve<Kundenliste>(new QueryEnvelope(new KundenlisteAbfrage())).Body;
        }

        public KundeInfo Kunde(Guid id)
        {
            return Kundenliste().Kunden.SingleOrDefault(_ => _.Id == id);
        }

        public void KundeErfassen(Guid id, string name, string anschrift)
        {
            _port.Handle(new CommandEnvelope(new KundeErfassen(kundenId:id, name:name, anschrift: anschrift)));
        }

        public void AnschriftAendern(Guid id, string neueanschrift)
        {
            _port.Handle(new CommandEnvelope(new AnschriftAendern(kundenId: id, neueAnschrift: neueanschrift)));
        }
    }
}