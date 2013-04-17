using System;

namespace Api.Bestellwesen.Aktionen
{
    public sealed class AuftragAusfuehren
    {
        public Guid AuftragId { get; set; }
        public DateTime AusfuehrungAvisiert { get; set; }
    }
}
