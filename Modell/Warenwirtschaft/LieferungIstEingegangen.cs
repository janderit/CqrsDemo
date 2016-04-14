using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class LieferungIstEingegangen
    {
        public Guid Lager;
        public Guid Produkt;
        public int Menge;

        public override string ToString()
        {
            return "Eine Warenlieferung für {ID:" + Produkt + "} ist in {ID:" + Lager + "} eingegangen (Menge " + Menge + ")";
        }
    }
}
