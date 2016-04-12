using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Frontend
{
    public class MainModul : CqrsGmbH_Web
    {
        public MainModul():base("")
        {
            Get["/"] = parameters => View["Startseite", Api().Kunden.Kundenliste()];
            Get["/log"] = parameters => View["log", Api().Meta.Protokoll().Eintraege];
        }
    }

    public class VertriebsModul : CqrsGmbH_Web
    {
        public VertriebsModul()
            : base("/vertrieb")
        {
            Get["/"] = parameters => View["Vertrieb"];
        }
    }

    public class LogistikModul : CqrsGmbH_Web
    {
        public LogistikModul()
            : base("/logistik")
        {
            Get["/"] = parameters => View["Logistik"];
        }
    }
}