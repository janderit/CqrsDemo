using System.Collections.Generic;

namespace Resourcen.Warenwirtschaft
{
    public sealed class Produktliste
    {
        public Produktliste()
        {
            Produkte=new List<ProduktInfo>();
        }

        public List<ProduktInfo> Produkte { get; set; }
    }
}
