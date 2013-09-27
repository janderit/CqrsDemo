using System;
using System.Collections.Generic;

namespace Resourcen.Shop
{
    public sealed class Warenkorb
    {
        public Guid Id;
        public Guid Kunde;
        public bool Leer {get { return Artikel.Count == 0; }}
        public List<ArtikelImWarenkorb> Artikel = new List<ArtikelImWarenkorb>();
    }

    public sealed class ArtikelImWarenkorb
    {
        public int Zeile;
        public Guid ZeileId;
        public Guid Produkt;
        public string Bezeichnung;
        public int Menge;
    }
}