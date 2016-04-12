using Infrastruktur.Messaging;
using Resourcen.Meta;

namespace Api.Meta
{
    public sealed class MetaProxy : MetaApi
    {
        private readonly Port _port;

        public MetaProxy(Port port)
        {
            _port = port;
        }

        public Protokoll Protokoll()
        {
            return _port.Retrieve<Protokoll>(new QueryEnvelope(new ProtokollAbfrage())).Body;
        }

    }
}