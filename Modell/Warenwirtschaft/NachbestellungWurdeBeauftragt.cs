using System;

namespace Modell.Warenwirtschaft
{
    public sealed class NachbestellungWurdeBeauftragt
    {
        public Guid Produkt;
        public int Menge;

        public override string ToString()
        {
            return "Eine Nachbestellung wurde beauftragt (Menge " + Menge + ")";
        }
    }
}
