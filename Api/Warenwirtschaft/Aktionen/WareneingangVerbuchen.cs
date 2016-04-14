using System;
using Infrastruktur.Messaging;

namespace Api.Warenwirtschaft.Aktionen
{
    public struct WareneingangVerbuchen : Command
    {
        public WareneingangVerbuchen(Guid lagerId, Guid produktId)
        {
            LagerId = lagerId;
            ProduktId = produktId;
        }

        public readonly Guid LagerId;
        public readonly Guid ProduktId;
    }
}
