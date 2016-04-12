using System.Collections.Generic;

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
