using System;

namespace Api.Warenkorb
{
    public interface WarenkorbApi
    {
        Resourcen.Shop.WarenkorbInfo FuerKunde(Guid id);
        void FuegeArtikelHinzu(Guid warenkorb, Guid produkt, int menge);
        void EntferneArtikel(Guid warenkorb, Guid zeile);
        void Bestellen(Guid warenkorb);
        void Leeren(Guid warenkorb);
    }
}
