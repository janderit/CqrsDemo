using System;
using System.Diagnostics;
using System.Linq;
using Resourcen.Warenwirtschaft;

namespace Modell
{
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

            ProduktExDenormalizer.Produkt(_sql, id, bezeichnung);
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