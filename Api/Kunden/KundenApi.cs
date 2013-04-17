using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Kunden.Abfragen;
using Api.Kunden.Aktionen;
using Infrastruktur;
using Infrastruktur.Messaging;
using Resourcen.Kunden;

namespace Api.Kunden
{
    public sealed class KundenApi
    {
        private readonly Port _port;

        public KundenApi(Port port)
        {
            _port = port;
        }

        public Kundenliste Kundenliste()
        {
            return _port.Retrieve<Kundenliste>(new Query()
                                                   {
                                                       Abfrage = new KundenlisteAbfrage()
                                                   }).Body;
        }

        public Kunde Kunde(Guid id)
        {
            return Kundenliste().Kunden.SingleOrDefault(_ => _.Id == id);
        }

        public Guid KundeErfassen(string name, string anschrift)
        {
            var id = Guid.NewGuid();
            _port.Handle(new Command
                             {
                                 Aktion = new KundeErfassen() {KundenId = id, Name = name, Anschrift = anschrift}
                             });
            return id;
        }

        public void AnschriftAendern(Guid id, string neueanschrift)
        {
            _port.Handle(new Command { Aktion = new AnschriftAendern {KundenId = id, NeueAnschrift = neueanschrift} });
        }
    }
}
