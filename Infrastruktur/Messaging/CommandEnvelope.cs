namespace Infrastruktur.Messaging
{
    public interface Command { }

    public struct CommandEnvelope
    {
        public CommandEnvelope(Command aktion)
        {
            Aktion = aktion;
        }

        public readonly Command Aktion;
    }
}
