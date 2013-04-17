using System;
using System.Collections.Generic;
using System.Text;

namespace Resourcen.Meta
{
    public sealed class Protokoll
    {
        public Protokoll()
        {
            Eintraege=new List<Eintrag>();
        }

        public List<Eintrag>  Eintraege { get; set; }

    }
}
