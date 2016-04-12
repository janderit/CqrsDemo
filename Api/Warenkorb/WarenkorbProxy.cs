using System;
using Api.Warenkorb.Abfragen;
using Api.Warenkorb.Aktionen;
using Infrastruktur.Messaging;

namespace Api.Warenkorb
{
    public sealed class WarenkorbProxy : WarenkorbApi
    {
        private readonly Port _port;

        public WarenkorbProxy(Port port)
        {
            _port = port;
        }

        public Resourcen.Shop.Warenkorb FuerKunde(Guid id)
        {
            return _port.Retrieve<Resourcen.Shop.Warenkorb>(new QueryEnvelope(new WarenkorbAbfrage(kunde: id))).Body;
        }

        public void FuegeArtikelHinzu(Guid warenkorb, Guid produkt, int menge)
        {
            _port.Handle(new CommandEnvelope(new ArtikelZuWarenkorbHinzufuegen(warenkorb: warenkorb, produkt: produkt, menge: menge)));
        }

        public void EntferneArtikel(Guid warenkorb, Guid zeile)
        {
            _port.Handle(new CommandEnvelope(new ArtikelAusWarenkorbEntfernen(warenkorb: warenkorb, zeile: zeile)));
        }

        public void Bestellen(Guid warenkorb)
        {
            _port.Handle(new CommandEnvelope(new WarenkorbBestellen(warenkorb: warenkorb)));
        }

        public void Leeren(Guid warenkorb)
        {
            _port.Handle(new CommandEnvelope(new WarenkorbLeeren(warenkorb: warenkorb)));
        }
    }
}