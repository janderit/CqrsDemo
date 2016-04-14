using System;
using Infrastruktur.Messaging;

namespace Api.Bestellwesen.Aktionen
{
    public struct AuftragAusfuehren : Command
    {
        public AuftragAusfuehren(Guid auftragId, Guid lagerId)
        {
            AuftragId = auftragId;
            LagerId = lagerId;
        }

        public readonly Guid LagerId;
        public readonly Guid AuftragId;
    }
}
