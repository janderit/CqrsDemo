using System.Collections.Generic;

namespace Resourcen.Warenwirtschaft
{
    public sealed class Lagerbestandsliste
    {
        public Lagerbestandsliste()
        {
            Bestand=new List<LagerbestandInfo>();
        }

        public List<LagerbestandInfo> Bestand { get; set; }
    }
}
