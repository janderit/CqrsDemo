using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastruktur.Messaging;

namespace Api.Warenwirtschaft.Aktionen
{
    public struct MindestVerfuegbarkeitDefinieren : Command
    {
        public MindestVerfuegbarkeitDefinieren(Guid produktId, int mindestVerfuegbarkeit, int mindestBestellmenge)
        {
            ProduktId = produktId;
            MindestVerfuegbarkeit = mindestVerfuegbarkeit;
            MindestBestellmenge = mindestBestellmenge;
        }

        public readonly Guid ProduktId;
        public readonly int MindestVerfuegbarkeit;
        public readonly int MindestBestellmenge;
    }
}
