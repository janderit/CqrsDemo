using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Kunden.Abfragen;
using Api.Kunden.Aktionen;
using Api.Warenkorb.Abfragen;
using Api.Warenkorb.Aktionen;
using Infrastruktur;
using Infrastruktur.Messaging;
using Resourcen.Kunden;

namespace Api.Warenkorb
{
    public sealed class WarenkorbApi
    {
        private readonly Port _port;

        public WarenkorbApi(Port port)
        {
            _port = port;
        }

        public Resourcen.Warenkorb.Warenkorb FuerKunde(Guid id)
        {
            return _port.Retrieve<Resourcen.Warenkorb.Warenkorb>(new Query()
                {
                    Abfrage = new WarenkorbAbfrage {Kunde = id}
                }).Body;
        }

        public void Fuege_Artikel_hinzu(Guid warenkorb, Guid produkt, int menge)
        {
            _port.Handle(new Command
                {
                    Aktion = new ArtikelZuWarenkorbHinzufuegen {Warenkorb = warenkorb, Produkt = produkt, Menge = menge}
                });
        }
    }
    
}
