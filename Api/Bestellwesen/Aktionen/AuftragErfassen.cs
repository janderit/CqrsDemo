using System;

namespace Api.Bestellwesen.Aktionen
{
    public sealed class AuftragErfassen
    {
        public Guid AuftragsId { get; set; }
        public Guid Kunde { get; set; }
        public Guid Produkt { get; set; }
        public int Menge { get; set; }
    }
}
