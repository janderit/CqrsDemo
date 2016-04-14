using System;
using System.Linq;
using Resourcen.Kunden;

namespace Modell
{
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
}