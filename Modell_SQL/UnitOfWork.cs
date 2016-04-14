using System;
using System.Data.SqlClient;

namespace Modell
{
    public class UnitOfWork : IDisposable
    {
        public readonly SQLOperations Db;

        public UnitOfWork(Func<SqlConnection> connectionFactory)
        {
            Db = new SQLOperations(connectionFactory, transactional: true);
        }

        public void Commit()
        {
            Db.Commit();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}