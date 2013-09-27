using System;

namespace Api.Warenkorb.Aktionen
{
    public sealed class ArtikelZuWarenkorbHinzufuegen
    {
        public Guid Warenkorb;
        public Guid Produkt;
        public int Menge;
    }
}