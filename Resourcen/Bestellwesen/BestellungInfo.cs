using System;

namespace Resourcen.Bestellwesen
{
    public sealed class BestellungInfo
    {
        public Guid Id { get; set; }
        public Guid Produkt { get; set; }
        public int Menge { get; set; }
        public string Produktname { get; set; }
        public Guid Kunde { get; set; }
        public string Kundenname { get; set; }
        public bool Erfuellt { get; set; }
    }
}
