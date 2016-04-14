using System;
using System.Linq;
using Resourcen.Warenwirtschaft;

namespace Modell
{
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

        public Lagerbestandsliste RetrieveAlleStandorte(Guid produkt)
        {
            return new Lagerbestandsliste
            {
                Bestand = _sql.Query(
                    @"SELECT [lager],[produkt],[bestand],[zulauf],[nachbestellen_ab] from [lagerposten] WHERE [produkt]=@produkt",
                    setpar =>
                    {
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
            ProduktExDenormalizer.Update(_sql, produkt);
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
            ProduktExDenormalizer.Update(_sql, produkt);
        }




    }
}