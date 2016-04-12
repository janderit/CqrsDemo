using System;
using Resourcen.Bestellwesen;

namespace Api.Bestellwesen
{
    public interface BestellwesenApi
    {
        Bestellungsliste OffeneBestellungen();
        void AuftragErfassen(Guid auftrag, Guid kunde, Guid produkt, int menge);
        void AuftragAusfuehren(Guid auftrag);
    }
}
