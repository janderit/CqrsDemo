using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Modell
{
    public class SQLOperations : IDisposable
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposed;

        public SQLOperations(Func<SqlConnection> connectionFactory, bool transactional)
        {
            _connection = connectionFactory();
            _connection.Open();
            if (transactional) _transaction = _connection.BeginTransaction();
        }

        public void ExecuteNonQuery(string sql, Action<Action<string, object>> set_parameter = null)
        {
            using (var cmd = new SqlCommand(sql, _connection, _transaction))
            {
                // ReSharper disable once AccessToDisposedClosure
                if (set_parameter != null) set_parameter((column, value) => cmd.Parameters.AddWithValue(column, value));
                Console.WriteLine("[SQL] " + Modify(cmd.CommandText, set_parameter));
                cmd.ExecuteNonQuery();
            }
        }

        private string Modify(string commandText, Action<Action<string, object>> setParameter)
        {
            if (setParameter == null) return commandText;
            setParameter((k, v) =>
            {
                commandText = commandText.Replace("@" + k, v.ToString());
            });
            return commandText;
        }

        public IEnumerable<T> Query<T>(string sql, Action<Action<string, object>> set_parameter, Func<SqlDataReader, T> map)
        {
            using (var cmd = new SqlCommand(sql, _connection, _transaction))
            {
                // ReSharper disable once AccessToDisposedClosure
                if (set_parameter != null) set_parameter((column, value) => cmd.Parameters.AddWithValue(column, value));
                Console.WriteLine("[SQL] " + Modify(cmd.CommandText, set_parameter));
                using (var dr = cmd.ExecuteReader()) while (dr.Read()) yield return map(dr);
            }
        }

        public void Commit()
        {
            if (_transaction != null) _transaction.Commit();
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            var transaction = _transaction;
            _transaction = null;
            if (transaction != null) transaction.Dispose();
            _connection.Dispose();
        }
    }
}