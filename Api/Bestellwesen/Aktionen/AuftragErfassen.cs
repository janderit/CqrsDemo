using System;
using Infrastruktur.Messaging;

namespace Api.Bestellwesen.Aktionen
{

    public struct AuftragErfassen : Command
    {
        public AuftragErfassen(Guid auftragsId, Guid kunde, Guid produkt, int menge)
        {
            AuftragsId = auftragsId;
            Kunde = kunde;
            Produkt = produkt;
            Menge = menge;
        }

        public readonly Guid AuftragsId;
        public readonly Guid Kunde;
        public readonly Guid Produkt;
        public readonly int Menge;
    }
}
