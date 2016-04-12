using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Messaging;

namespace Api.Warenwirtschaft.Aktionen
{
    public struct WareneingangVerbuchen : Command
    {
        public WareneingangVerbuchen(Guid produktId)
        {
            ProduktId = produktId;
        }

        public readonly Guid ProduktId;
    }
}
