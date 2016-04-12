using System;
using Infrastruktur.Messaging;

namespace Api.Warenkorb.Aktionen
{
    public struct WarenkorbBestellen : Command
    {
        public WarenkorbBestellen(Guid warenkorb)
        {
            Warenkorb = warenkorb;
        }

        public readonly Guid Warenkorb;
    }
}