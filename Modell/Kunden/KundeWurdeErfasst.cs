using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Kunden
{
    public sealed class KundeWurdeErfasst
    {
        public Guid Kunde;
        public string Name;
        public string Anschrift;

        public override string ToString()
        {
            return "Kunde " + Name + " wurde mit Anschrift " + Anschrift + " erfasst.";
        }
    }
}
