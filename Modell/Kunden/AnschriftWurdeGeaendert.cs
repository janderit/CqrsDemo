
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Kunden
{
    public sealed class AnschriftWurdeGeaendert
    {
        public string NeueAnschrift { get; set; }

        public override string ToString()
        {
            return "Die Anschrift des Kunden wurde geändert: "+NeueAnschrift;
        }
    }
}
