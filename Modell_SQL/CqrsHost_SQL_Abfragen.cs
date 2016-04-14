using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Api.Bestellwesen.Abfragen;
using Api.Bestellwesen.Aktionen;
using Api.Kunden.Abfragen;
using Api.Kunden.Aktionen;
using Api.Meta;
using Api.Warenkorb.Abfragen;
using Api.Warenkorb.Aktionen;
using Api.Warenwirtschaft.Abfragen;
using Api.Warenwirtschaft.Aktionen;
using Infrastruktur.Messaging;
using Resourcen.Bestellwesen;
using Resourcen.Kunden;
using Resourcen.Meta;
using Resourcen.Shop;
using Resourcen.Warenwirtschaft;

namespace Modell
{
    partial class CqrsHost_SQL
    {
        private Kundenliste Handle(QueryEnvelope queryEnvelope, KundenlisteAbfrage abfrage)
        {
            using (var db = ReadAccess())
            {
                return new KundenRepository(db).Retrieve();
            }
        }

        private Produktliste Handle(QueryEnvelope queryEnvelope, ProduktlisteAbfrage abfrage)
        {
            using (var db = ReadAccess())
            {
                return new ProduktRepository(db).Retrieve();
            }
        }

        /*private ProduktlisteEx Handle(QueryEnvelope queryEnvelope, ProduktlisteExAbfrage abfrage)
        {
            using (var db = ReadAccess())
            {
                var lagerbestand =
                    new LagerbestandRepository(db).RetrieveAlleStandorte()
                    .Bestand.GroupBy(_=>_.Produkt).ToDictionary(_=>_.Key, _=>new { Bestand = _.Sum(x=>x.LagerBestand), Zulauf = _.Sum(x => x.MengeImZulauf) });

                return new ProduktlisteEx {Produkte = new ProduktRepository(db).Retrieve().Produkte.Select(
                    p => new ProduktInfoEx
                    {
                        Id = p.Id,
                        Bezeichnung = p.Bezeichnung,
                        LagerBestand = lagerbestand.ContainsKey(p.Id) ? lagerbestand[p.Id].Bestand : 0,
                        Verfuegbar = lagerbestand.ContainsKey(p.Id) ? lagerbestand[p.Id].Bestand+lagerbestand[p.Id].Zulauf : 0,
                    }
                    ).ToList()};
            }
        }*/


        private ProduktlisteEx Handle(QueryEnvelope queryEnvelope, ProduktlisteExAbfrage abfrage)
        {
            using (var db = ReadAccess())
            {
                return new ProduktlisteEx
                {
                    Produkte =
                        db.Query(
                            @"SELECT [Id], [Bezeichnung], [bestand], [verfuegbar] from [produkteex]",
                            null,
                            dr => new ProduktInfoEx
                            {
                                Id = dr.GetGuid(0),
                                Bezeichnung = dr.GetString(1),
                                LagerBestand = dr.GetInt32(2),
                                Verfuegbar = dr.GetInt32(3)
                            }
                            ).ToList()
                };
            }
        }



        private Lagerbestandsliste Handle(QueryEnvelope queryEnvelope, LagerbestandsAbfrage abfrage)
        {

            using (var db = ReadAccess())
            {
                return new LagerbestandRepository(db).Retrieve(abfrage.LagerId);
            }
        }








        private Bestellungenliste Handle(QueryEnvelope queryEnvelope, OffeneBestellungenAbfrage abfrage)
        {
            throw new NotImplementedException();
        }

        private Protokoll Handle(QueryEnvelope queryEnvelope, ProtokollAbfrage abfrage)
        {
            return new Protokoll
            {
                Eintraege = new List<Eintrag>()
            };
        }

        private string ReplaceGuidsWith(string input, Func<Guid, string> replacements)
        {
            const string guidPattern = @"(\{ID:([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\})";
            return Regex.Replace(input, guidPattern, match => replacements(MatchedGuid(match)));
        }

        private static Guid MatchedGuid(Match match)
        {
            return new Guid(match.Value.Substring(4, 36));
        }


        private WarenkorbInfo Handle(QueryEnvelope queryEnvelope, WarenkorbAbfrage abfrage)
        {
            throw new NotImplementedException();
        }
    }
}