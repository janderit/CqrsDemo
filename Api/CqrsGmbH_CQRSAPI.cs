using System;
using Api.Bestellwesen;
using Api.Kunden;
using Api.Meta;
using Api.Warenkorb;
using Api.Warenwirtschaft;
using Infrastruktur.Messaging;

namespace Api
{
    public sealed class CqrsGmbH_CQRSAPI : CqrsGmbH
    {
        public CqrsGmbH_CQRSAPI(Port port)
        {
            if (port == null) throw new ArgumentNullException("port");

            Kunden = new KundenProxy(port);
            Warenkorb = new WarenkorbProxy(port);
            Warenwirtschaft = new WarenwirtschaftProxy(port);
            Bestellwesen = new BestellwesenProxy(port);
            Meta = new MetaProxy(port);
        }

        public override KundenApi Kunden { get; protected set; }
        public override WarenkorbApi Warenkorb { get; protected set; }
        public override WarenwirtschaftApi Warenwirtschaft { get; protected set; }
        public override BestellwesenApi Bestellwesen { get; protected set; }
        public override MetaApi Meta { get; protected set; }
    }
}