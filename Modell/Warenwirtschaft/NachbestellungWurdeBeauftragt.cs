using System;

namespace Modell.Warenwirtschaft
{
    public sealed class NachbestellungWurdeBeauftragt
    {
        public int Menge { get; set; }

        public override string ToString()
        {
            return "Eine Nachbestellung wurde beauftragt (Menge " + Menge + ")";
        }
    }
}
