using System;
using System.Collections.Generic;

namespace Resourcen.Shop
{
    public sealed class WarenkorbInfo
    {
        public Guid Id;
        public Guid Kunde;
        public bool Leer {get { return Artikel.Count == 0; }}
        public List<ArtikelImWarenkorb> Artikel = new List<ArtikelImWarenkorb>();
    }
}