namespace Infrastruktur.Messaging
{
    public interface Query { }

    public struct QueryEnvelope
    {
        public QueryEnvelope(Query abfrage)
        {
            Abfrage = abfrage;
        }

        public readonly Query Abfrage;
    }
}
