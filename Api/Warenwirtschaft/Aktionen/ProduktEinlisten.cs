using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Messaging;

namespace Api.Warenwirtschaft.Aktionen
{
    public struct ProduktEinlisten : Command
    {
        public ProduktEinlisten(Guid produktId, string bezeichnung)
        {
            ProduktId = produktId;
            Bezeichnung = bezeichnung;
        }

        public readonly Guid ProduktId;
        public readonly string Bezeichnung;
    }
}
