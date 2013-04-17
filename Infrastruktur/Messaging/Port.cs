using Infrastruktur.Common;

namespace Infrastruktur.Messaging
{
    public interface Port
    {
        void Handle(Command command);
        Resource<T> Retrieve<T>(Query query) where T:class;
    }
}
