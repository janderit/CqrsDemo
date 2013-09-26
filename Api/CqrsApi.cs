using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Bestellwesen;
using Api.Kunden;
using Api.Meta;
using Api.Warenkorb;
using Api.Warenwirtschaft;
using Infrastruktur;
using Infrastruktur.Messaging;
using Resourcen.Kunden;

namespace Api
{
    public sealed class CqrsApi
    {
        private readonly Port _port;

        public CqrsApi(Port port)
        {
            if (port == null) throw new ArgumentNullException("port");
            _port = port;
        }

        

        public KundenApi Kunden { get { return new KundenApi(_port); } }
        public WarenkorbApi Warenkorb { get { return new WarenkorbApi(_port); } }
        public WarenwirtschaftApi Warenwirtschaft { get { return new WarenwirtschaftApi(_port); } }
        public BestellwesenApi Bestellwesen { get { return new BestellwesenApi(_port); } }
        public MetaApi Meta { get { return new MetaApi(_port); } }

    }
}
