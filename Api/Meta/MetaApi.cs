using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Bestellwesen.Abfragen;
using Api.Bestellwesen.Aktionen;
using Api.Kunden.Abfragen;
using Api.Kunden.Aktionen;
using Infrastruktur;
using Infrastruktur.Messaging;
using Resourcen.Bestellwesen;
using Resourcen.Kunden;
using Resourcen.Meta;

namespace Api.Meta
{
    public sealed class MetaApi
    {
        private readonly Port _port;

        public MetaApi(Port port)
        {
            _port = port;
        }

        public Protokoll Protokoll()
        {
            return _port.Retrieve<Protokoll>(new Query() { Abfrage = new ProtokollAbfrage() }).Body;
        }

    }
}
