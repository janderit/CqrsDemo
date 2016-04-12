using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Messaging;

namespace Api.Warenwirtschaft.Aktionen
{
    public struct NachbestellungBeauftragen : Command
    {
        public NachbestellungBeauftragen(Guid produktId, int bestellteMenge)
        {
            ProduktId = produktId;
            BestellteMenge = bestellteMenge;
        }

        public readonly Guid ProduktId;
        public readonly int BestellteMenge;
    }
}
