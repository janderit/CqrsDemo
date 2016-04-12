using System;
using System.Collections.Generic;
using System.Text;

namespace Resourcen.Warenwirtschaft
{
    public sealed class Produktliste
    {
        public Produktliste()
        {
            Produkte=new List<ProduktInfo>();
        }

        public List<ProduktInfo>  Produkte { get; set; }
    }
}
