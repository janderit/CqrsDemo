using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Kunden
{
    public sealed class KundeWurdeErfasst
    {
        public string Name { get; set; }
        public string Anschrift { get; set; }

        public override string ToString()
        {
            return "Kunde wurde mit Anschrift "+Anschrift+" erfasst.";
        }
    }
}
