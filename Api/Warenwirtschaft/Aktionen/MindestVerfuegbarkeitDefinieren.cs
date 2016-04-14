using System;
using Infrastruktur.Messaging;

namespace Api.Warenwirtschaft.Aktionen
{
    public struct MindestVerfuegbarkeitDefinieren : Command
    {
        public MindestVerfuegbarkeitDefinieren(Guid lagerId, Guid produktId, int mindestVerfuegbarkeit, int mindestBestellmenge)
        {
            LagerId = lagerId;
            ProduktId = produktId;
            MindestVerfuegbarkeit = mindestVerfuegbarkeit;
            MindestBestellmenge = mindestBestellmenge;
        }

        public readonly Guid LagerId;
        public readonly Guid ProduktId;
        public readonly int MindestVerfuegbarkeit;
        public readonly int MindestBestellmenge;
    }
}
