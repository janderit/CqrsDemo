using System;
using System.Linq;

namespace Modell
{
    public static class ProduktExDenormalizer
    {
        public static void Produkt(SQLOperations sql, Guid id, string bezeichnung)
        {
            sql.ExecuteNonQuery(
                @"INSERT INTO [produkteex] ([id],[bezeichnung],[bestand],[verfuegbar]) VALUES (@id,@bezeichnung,0,0)",
                set_parameter =>
                {
                    set_parameter("id", id);
                    set_parameter("bezeichnung", bezeichnung);
                }
                );
        }

        public static void Update(SQLOperations sql, Guid produkt)
        {
            var posten = new LagerbestandRepository(sql).RetrieveAlleStandorte(produkt);
            var bestand = posten.Bestand.Sum(_ => _.LagerBestand);
            var zulauf = posten.Bestand.Sum(_ => _.MengeImZulauf);

            var verfuegbar = bestand + zulauf;

            sql.ExecuteNonQuery(
                @"UPDATE [produkteex] SET [bestand]=@bestand,[verfuegbar]=@verfuegbar WHERE [id]=@produkt",
                set_parameter =>
                {
                    set_parameter("produkt", produkt);
                    set_parameter("bestand", bestand);
                    set_parameter("verfuegbar", verfuegbar);
                }
                );
        }
    }
}