using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastruktur.Common
{
    public class VorgangNichtAusgefuehrt : ApplicationException
    {
        public VorgangNichtAusgefuehrt(string message):base(message){}
    }
}
