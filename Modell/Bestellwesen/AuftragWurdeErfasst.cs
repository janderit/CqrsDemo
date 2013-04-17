using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Bestellwesen
{
    public sealed class AuftragWurdeErfasst
    {
        public Guid Produkt { get; set; }
        public Guid Kunde { get; set; }
        public int Menge { get; set; }

        public override string ToString()
        {
            return "Ein Auftrag über " + Menge + "x " + Produkt + " für " + Kunde + " wurde erfasst.";
        }
    }

    
}
