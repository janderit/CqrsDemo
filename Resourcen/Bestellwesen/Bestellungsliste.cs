using System;
using System.Collections.Generic;
using System.Text;

namespace Resourcen.Bestellwesen
{
    public sealed class Bestellungsliste
    {
        public Bestellungsliste()
        {
            Bestellungen= new List<Bestellung>();
        }
        public List<Bestellung> Bestellungen { get; set; }
    }
}
