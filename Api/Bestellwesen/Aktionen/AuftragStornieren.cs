using System;
using Infrastruktur.Messaging;

namespace Api.Bestellwesen.Aktionen
{
    public struct AuftragStornieren : Command
    {
        public AuftragStornieren(Guid auftrag) 
        {
            Auftrag = auftrag;
        }

        public readonly Guid Auftrag;

    }
}
