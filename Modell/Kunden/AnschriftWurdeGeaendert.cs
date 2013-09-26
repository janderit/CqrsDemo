
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Kunden
{
    public sealed class AnschriftWurdeGeaendert
    {
        public Guid Kunde;
        public string NeueAnschrift;

        public override string ToString()
        {
            return "Die Anschrift des Kunden wurde geändert: "+NeueAnschrift;
        }
    }
}
