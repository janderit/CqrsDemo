using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public class WarenWurdenAusgeliefert
    {
        public int Menge { get; set; }

        public override string ToString()
        {
            return "Waren wurden ausgeliefert (Menge " + Menge + ")";
        }
    }
}
