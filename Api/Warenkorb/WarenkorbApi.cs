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

        public Resourcen.Shop.Warenkorb FuerKunde(Guid id)
        {
            return _port.Retrieve<Resourcen.Shop.Warenkorb>(new Query()
                {
                    Abfrage = new WarenkorbAbfrage {Kunde = id}
                }).Body;
        }

        public void FuegeArtikelHinzu(Guid warenkorb, Guid produkt, int menge)
        {
            _port.Handle(new Command
                {
                    Aktion = new ArtikelZuWarenkorbHinzufuegen {Warenkorb = warenkorb, Produkt = produkt, Menge = menge}
                });
        }

        public void EntferneArtikel(Guid warenkorb, Guid zeile)
        {
            _port.Handle(new Command
                {
                    Aktion = new ArtikelAusWarenkorbEntfernen() {Warenkorb = warenkorb, Zeile = zeile}
                });
        }

        public void Bestellen(Guid warenkorb)
        {
            _port.Handle(new Command
            {
                Aktion = new WarenkorbBestellen() { Warenkorb = warenkorb }
            });
        }

        public void Leeren(Guid warenkorb)
        {
            _port.Handle(new Command
            {
                Aktion = new WarenkorbLeeren() { Warenkorb = warenkorb }
            });
        }
    }
    
}
