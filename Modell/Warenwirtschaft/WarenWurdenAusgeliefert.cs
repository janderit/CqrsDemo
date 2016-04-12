using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public class WarenWurdenAusgeliefert
    {
        public Guid Produkt;
        public int Menge;

        public override string ToString()
        {
            return "Waren wurden ausgeliefert (" + Menge + "x {ID:" + Produkt + "}).";
        }
    }
}
