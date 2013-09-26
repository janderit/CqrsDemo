using System;

namespace Modell.Warenkorb
{
    public sealed class ArtikelWurdeZuWarenkorbHinzugefuegt
    {
        public Guid Warenkorb;
        public Guid Produkt;
        public int Menge;
    }
}