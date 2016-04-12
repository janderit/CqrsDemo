using System;
using Infrastruktur.Messaging;

namespace Api.Bestellwesen.Aktionen
{
    public struct AuftragAusfuehren : Command
    {
        public AuftragAusfuehren(Guid auftragId)
        {
            AuftragId = auftragId;
        }

        public readonly Guid AuftragId;
    }
}
