using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class WareWurdeDisponiert
    {
        public int Menge { get; set; }

        public override string ToString()
        {
            return "Ware wurde für einen Auftrag disponiert (Menge "+Menge+")";
        }
    }
}
