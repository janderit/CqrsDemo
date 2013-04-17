using System;
using System.Collections.Generic;
using System.Text;

namespace Resourcen.Kunden
{
    public sealed class Kundenliste
    {
        public Kundenliste()
        {
            Kunden = new List<Kunde>();
        }

        public List<Kunde> Kunden { get; set; }
    }
}
