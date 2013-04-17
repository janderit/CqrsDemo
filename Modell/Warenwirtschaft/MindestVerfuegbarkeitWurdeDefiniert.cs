using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class MindestBestellmengeWurdeDefiniert
    {
        public int Menge { get; set; }
        public override string ToString()
        {
            return "Für das Produkt wurde die Mindestbestellmenge definiert: "+Menge;
        }
    }
}
