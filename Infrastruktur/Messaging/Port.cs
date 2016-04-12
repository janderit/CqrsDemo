using Infrastruktur.Common;

namespace Infrastruktur.Messaging
{
    public interface Port
    {
        void Handle(CommandEnvelope commandEnvelope);
        Resource<T> Retrieve<T>(QueryEnvelope queryEnvelope);
    }
}
