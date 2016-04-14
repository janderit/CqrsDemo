using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Resourcen.Kunden;
using Resourcen.Warenwirtschaft;

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


    public class KundenRepository
    {
        private readonly SQLOperations _sql;

        public KundenRepository(UnitOfWork uow)
        {
            _sql = uow.Db;
        }

        public KundenRepository(SQLOperations sql)
        {
            _sql = sql;
        }

        public void Insert(Guid id, string name, string anschrift)
        {
            _sql.ExecuteNonQuery(
                @"INSERT INTO [Kunden] ([Id], [Name], [Anschrift]) VALUES (@id, @name, @anschrift)",
                set_parameter =>
                {
                    set_parameter("id", id);
                    set_parameter("name", name);
                    set_parameter("anschrift", anschrift);
                }
                );
        }

        public Kundenliste Retrieve()
        {
            return new Kundenliste
            {
                Kunden = _sql.Query(
                    @"SELECT [Id], [Name], [Anschrift] from [Kunden]",
                    null,
                    dr => new KundeInfo
                    {
                        Id = dr.GetGuid(0),
                        Name = dr.GetString(1),
                        Anschrift = dr.GetString(2)
                    }
                    ).ToList()
            };
        }

        public Kundenliste Retrieve(Guid id)
        {
            return new Kundenliste
            {
                Kunden = _sql.Query(
                    @"SELECT [Id], [Name], [Anschrift] from [Kunden] WHERE [id]=@id",
                    setpar => setpar("id", id),
                    dr => new KundeInfo
                    {
                        Id = dr.GetGuid(0),
                        Name = dr.GetString(1),
                        Anschrift = dr.GetString(2)
                    }
                    ).ToList()
            };
        }

        public void Update(KundeInfo kunde)
        {
            Delete(kunde.Id);
            Insert(kunde.Id, kunde.Name, kunde.Anschrift);
        }

        public void Delete(Guid id)
        {
            _sql.ExecuteNonQuery(@"DELETE FROM [kunden] WHERE [Id]=@id", setpar => setpar("id", id));
        }
    }


    public class LagerbestandRepository
    {
        private readonly SQLOperations _sql;

        public LagerbestandRepository(UnitOfWork uow)
        {
            _sql = uow.Db;
        }

        public LagerbestandRepository(SQLOperations sql)
        {
            _sql = sql;
        }

        public Lagerbestandsliste RetrieveAlleStandorte()
        {
            return new Lagerbestandsliste
            {
                Bestand = _sql.Query(
                    @"SELECT [lager],[produkt],[bestand],[zulauf],[nachbestellen_ab] from [lagerposten]",
                    null,
                    dr =>
                    {
                        var zulauf = dr.GetInt32(3);
                        var nachbestellen = dr.GetInt32(4);
                        return new LagerbestandInfo
                        {
                            Lager = dr.GetGuid(0),
                            Produkt = dr.GetGuid(1),
                            LagerBestand = dr.GetInt32(2),
                            MengeImZulauf = zulauf,
                            Produktbezeichnung = "",
                            AutomatischeNachbestellungen = nachbestellen > 0,
                            Nachbestellt = zulauf > 0
                        };
                    }).ToList()
            };
        }

        public Lagerbestandsliste Retrieve(Guid lager)
        {
            return new Lagerbestandsliste
            {
                Bestand = _sql.Query(
                    @"SELECT [lager],[produkt],[bestand],[zulauf],[nachbestellen_ab] from [lagerposten] WHERE [lager]=@lager",
                    setpar =>
                    {
                        setpar("lager", lager);
                    },
                    dr =>
                    {
                        var zulauf = dr.GetInt32(3);
                        var nachbestellen = dr.GetInt32(4);
                        return new LagerbestandInfo
                        {
                            Lager = dr.GetGuid(0),
                            Produkt = dr.GetGuid(1),
                            LagerBestand = dr.GetInt32(2),
                            MengeImZulauf = zulauf,
                            Produktbezeichnung = "",
                            AutomatischeNachbestellungen = nachbestellen > 0,
                            Nachbestellt = zulauf > 0
                        };
                    }).ToList()
            };
        }

        public Lagerbestandsliste Retrieve(Guid lager, Guid produkt)
        {
            return new Lagerbestandsliste
            {
                Bestand = _sql.Query(
                    @"SELECT [lager],[produkt],[bestand],[zulauf],[nachbestellen_ab] from [lagerposten] WHERE [lager]=@lager AND [produkt]=@produkt",
                    setpar =>
                    {
                        setpar("lager", lager);
                        setpar("produkt", produkt);
                    },
                    dr =>
                    {
                        var zulauf = dr.GetInt32(3);
                        var nachbestellen = dr.GetInt32(4);
                        return new LagerbestandInfo
                        {
                            Lager = dr.GetGuid(0),
                            Produkt = dr.GetGuid(1),
                            LagerBestand = dr.GetInt32(2),
                            MengeImZulauf = zulauf,
                            Produktbezeichnung = "",
                            AutomatischeNachbestellungen = nachbestellen > 0,
                            Nachbestellt = zulauf > 0
                        };
                    }).ToList()
            };
        }

        public void Insert(Guid lager, Guid produkt)
        {
            _sql.ExecuteNonQuery(
                @"INSERT INTO [lagerposten] ([lager],[produkt],[bestand],[zulauf],[nachbestellen_ab],[nachbestellen_menge]) VALUES (@lager, @produkt, 0, 0, 0, 0)",
                setpar =>
                {
                    setpar("lager", lager);
                    setpar("produkt", produkt);
                }
                );
        }

        public void Set_Zulauf(Guid lager, Guid produkt, int zulauf)
        {
            _sql.ExecuteNonQuery(
                @"UPDATE [lagerposten] SET [zulauf]=@zulauf WHERE [lager]=@lager AND [produkt]=@produkt",
                setpar =>
                {
                    setpar("lager", lager);
                    setpar("produkt", produkt);
                    setpar("zulauf", zulauf);
                }
                );
        }

        public void Set_Bestand(Guid lager, Guid produkt, int bestand)
        {
            _sql.ExecuteNonQuery(
                @"UPDATE [lagerposten] SET [bestand]=@bestand WHERE [lager]=@lager AND [produkt]=@produkt",
                setpar =>
                {
                    setpar("lager", lager);
                    setpar("produkt", produkt);
                    setpar("bestand", bestand);
                }
                );
        }




    }

    public class ProduktRepository
    {
        private readonly SQLOperations _sql;

        public ProduktRepository(UnitOfWork uow)
        {
            _sql = uow.Db;
        }

        public ProduktRepository(SQLOperations sql)
        {
            _sql = sql;
        }

        public void Insert(Guid id, string bezeichnung)
        {
            _sql.ExecuteNonQuery(
                @"INSERT INTO [Produkte] ([Id], [Bezeichnung]) VALUES (@id, @bezeichnung)",
                set_parameter =>
                {
                    set_parameter("id", id);
                    set_parameter("bezeichnung", bezeichnung);
                }
                );
        }

        public Produktliste Retrieve()
        {
            return new Produktliste
            {
                Produkte = _sql.Query(
                    @"SELECT [Id], [Bezeichnung] from [Produkte]",
                    null,
                    dr => new ProduktInfo
                    {
                        Id = dr.GetGuid(0),
                        Bezeichnung = dr.GetString(1)
                    }
                    ).ToList()
            };
        }

        public Produktliste Retrieve(Guid id)
        {
            return new Produktliste
            {
                Produkte = _sql.Query(
                    @"SELECT [Id], [Bezeichnung] from [Produkte] WHERE [id]=id",
                    setpar => setpar("id", id),
                    dr => new ProduktInfo
                    {
                        Id = dr.GetGuid(0),
                        Bezeichnung = dr.GetString(1)
                    }
                    ).ToList()
            };
        }
    }
}