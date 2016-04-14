using System.Collections.Generic;

namespace Resourcen.Warenwirtschaft
{
    public sealed class ProduktlisteEx
    {
        public ProduktlisteEx()
        {
            Produkte = new List<ProduktInfoEx>();
        }

        public List<ProduktInfoEx> Produkte { get; set; }
    }
}