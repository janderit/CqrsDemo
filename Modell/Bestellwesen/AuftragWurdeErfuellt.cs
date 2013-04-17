using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Bestellwesen
{
    public sealed class AuftragWurdeErfuellt
    {
        public Guid Produkt { get; set; }
        public int Menge { get; set; }

        public override string ToString()
        {
            return "Ein Auftrag über " + Menge + "x " + Produkt + " wurde erfüllt";
        }
    }
}
