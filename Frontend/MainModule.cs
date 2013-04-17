using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Frontend
{
    public class MainModule : CqrsGmbh
    {
        public MainModule():base("")
        {
            Get["/"] = parameters => View["Startseite"];
            Get["/log"] = parameters => View["log", Api().Meta.Protokoll().Eintraege];
        }
    }
}