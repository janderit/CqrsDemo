using System;
using Infrastruktur.Messaging;

namespace Api.Warenwirtschaft.Abfragen
{
    public struct LagerbestandsAbfrage : Query
    {
        public Guid LagerId { get; set; }
    }
}
