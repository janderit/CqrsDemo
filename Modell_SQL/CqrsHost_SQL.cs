using System;
using System.Data.SqlClient;
using Infrastruktur.Common;
using Infrastruktur.Messaging;

namespace Modell
{
    public partial class CqrsHost_SQL : Port
    {
        private readonly Func<SqlConnection> _connectionFactory;

        public CqrsHost_SQL(Func<SqlConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(CommandEnvelope commandEnvelope)
        {
            using (var unitOfWork = new UnitOfWork(_connectionFactory))
            {
                Handle(commandEnvelope, (dynamic) commandEnvelope.Aktion, unitOfWork);
                unitOfWork.Commit();
            }
        }

        public Resource<T> Retrieve<T>(QueryEnvelope queryEnvelope)
        {
            var resource = Handle(queryEnvelope, (dynamic)queryEnvelope.Abfrage);
            if (resource == null)
            {
                throw new InvalidOperationException(string.Format("Query Handler liefert kein Ergebnis! {0}",
                                                                  queryEnvelope.Abfrage.GetType().Name));
            }

            if (resource is Resource<T>)
            {
                return (Resource<T>)resource;
            }

            var t = (T)resource;
            if (t == null)
            {
                throw new InvalidOperationException(
                    string.Format("Query Handler liefert unerwartetes Ergebnis! {0} {1} statt {2}",
                                  queryEnvelope.Abfrage.GetType().Name, resource.GetType().Name, typeof(T).Name));
            }

            return new Resource<T> { Body = t };
        }

        private void Handle(CommandEnvelope commandEnvelope, object aktion, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException(string.Format("Kein Command Handler für {0} definiert", aktion.GetType().Name));
        }

        private object Handle(QueryEnvelope queryEnvelope, object abfrage)
        {
            throw new NotImplementedException(string.Format("Kein Query Handler für {0} definiert", abfrage.GetType().Name));
        }

        private SQLOperations ReadAccess()
        {
            return new SQLOperations(_connectionFactory, transactional:false);
        }
    }
}