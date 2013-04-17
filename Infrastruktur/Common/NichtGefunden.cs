using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastruktur.Common
{
    public class NichtGefunden : VorgangNichtAusgefuehrt
    {
        public NichtGefunden(string typ) : base(string.Format("{0} wurde nicht gefunden.", typ)) { }
    }
}
