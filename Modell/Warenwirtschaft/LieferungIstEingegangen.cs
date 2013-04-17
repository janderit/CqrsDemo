using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class LieferungIstEingegangen
    {
        public int Menge { get; set; }

        public override string ToString()
        {
            return "Eine Warenlieferung ist eingegangen (Menge " + Menge + ")";
        }
    }
}
