using System;

namespace Modell.Shop
{
    public sealed class ArtikelWurdeZuWarenkorbHinzugefuegt
    {
        public Guid Zeile;
        public Guid Warenkorb;
        public Guid Produkt;
        public int Menge;

        public override string ToString()
        {
            return "Artikel {ID:" + Produkt + "} (" + Menge + "x) zu Warenkorb {ID:" + Warenkorb + "} hinzugefügt";
        }
    }
}