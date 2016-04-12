using System.Collections.Generic;

namespace Resourcen.Bestellwesen
{
    public sealed class Bestellungenliste
    {
        public Bestellungenliste()
        {
            Bestellungen= new List<BestellungInfo>();
        }
        public List<BestellungInfo> Bestellungen { get; set; }
    }
}
