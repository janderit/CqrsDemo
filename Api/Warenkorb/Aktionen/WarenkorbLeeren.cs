using System;
using Infrastruktur.Messaging;

namespace Api.Warenkorb.Aktionen
{
    public struct WarenkorbLeeren : Command
    {
        public WarenkorbLeeren(Guid warenkorb)
        {
            Warenkorb = warenkorb;
        }

        public readonly Guid Warenkorb;
    }
}