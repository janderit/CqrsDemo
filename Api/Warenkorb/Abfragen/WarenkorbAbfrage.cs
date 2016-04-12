using System;
using Infrastruktur.Messaging;

namespace Api.Warenkorb.Abfragen
{
    public struct WarenkorbAbfrage : Query
    {
        public WarenkorbAbfrage(Guid kunde)
        {
            Kunde = kunde;
        }

        public readonly Guid Kunde;
    }
}
