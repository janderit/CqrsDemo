using System;

namespace Modell.Warenwirtschaft
{
    public sealed class NachbestellungWurdeBeauftragt
    {
        public Guid Lager;
        public Guid Produkt;
        public int Menge;

        public override string ToString()
        {
            return "Eine Nachbestellung über " + Menge
                + "x {ID:" + Produkt + "} nach {ID:" + Lager + "} wurde beauftragt.";
        }
    }
}
