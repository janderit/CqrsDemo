using System;
using System.Collections.Generic;
using System.Text;

namespace Resourcen.Kunden
{
    public sealed class Kundenliste
    {
        public Kundenliste()
        {
            Kunden = new List<KundeInfo>();
        }

        public List<KundeInfo> Kunden { get; set; }
    }
}
