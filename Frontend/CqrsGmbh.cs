using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api;
using Host;
using Infrastruktur;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Infrastruktur.Messaging;
using Nancy;
using Nancy.Responses;

namespace Frontend
{
    public abstract class CqrsGmbh : NancyModule
    {

        static CqrsGmbh()
        {
            var store = new InMemoryEventStore();
            Port = new CqrsHost(store);
            var api = new CqrsApi(Port);
            api.Kunden.KundeErfassen("Jens Mustermann", "Musterstrasse 10, 12345 Musterstadt");
            var honig = api.Warenwirtschaft.Einlisten("Honig");
            api.Warenwirtschaft.Nachbestellen(honig, 100);
            api.Warenwirtschaft.Wareneingang(honig);
        }

        private static readonly Port Port;

        protected CqrsGmbh(string basepath):base(basepath)
        {            
        }

        protected CqrsApi Api()
        {
            return new CqrsApi(Port);
        }
    }
}