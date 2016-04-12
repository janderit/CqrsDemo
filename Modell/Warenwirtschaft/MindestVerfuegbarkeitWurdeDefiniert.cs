using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class MindestBestellmengeWurdeDefiniert
    {
        public Guid Produkt;
        public int Menge;

        public override string ToString()
        {
            return "Für das Produkt {ID:" + Produkt + "} wurde die Mindestbestellmenge definiert: " + Menge + ".";
        }
    }
}
