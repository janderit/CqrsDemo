using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Api;
using Modell;
using Modell_shared;
using NUnit.Framework;
using Resourcen.Bestellwesen;
using Resourcen.Kunden;
using Resourcen.Shop;
using Resourcen.Warenwirtschaft;

namespace Spezifikation
{
    public abstract class Spezifikation_CQRS_SQL
    {
        static readonly string ServerConnectionString = "Server=LOCALHOST\\SQL2014;Trusted_Connection=True;";
        private static readonly string MasterConnectionString = ServerConnectionString + "Database=master;";
        private readonly string TestDatabase;
        private readonly string TestConnectionString;

        protected Spezifikation_CQRS_SQL()
        {
            TestDatabase = "cqrsdemo_test";
            TestConnectionString = ServerConnectionString + "Database=" + TestDatabase + ";";
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            try
            {
                using (var db = new SQLOperations(() => new SqlConnection(MasterConnectionString), transactional: false)
                    )
                {
                    db.ExecuteNonQuery(string.Format(@"CREATE DATABASE [{0}];", TestDatabase));
                }

                using (var db = new SQLOperations(() => new SqlConnection(TestConnectionString), transactional: false))
                {
                    db.ExecuteNonQuery(@"CREATE TABLE [kunden] ( id uniqueidentifier NOT NULL, name nvarchar(100) NOT NULL, anschrift nvarchar(200) NOT NULL)");
                    db.ExecuteNonQuery(@"CREATE TABLE [produkte] ( id uniqueidentifier NOT NULL, bezeichnung nvarchar(100) NOT NULL)");
                    db.ExecuteNonQuery(@"CREATE TABLE [lagerposten] ( lager uniqueidentifier NOT NULL, produkt uniqueidentifier NOT NULL, bestand int NOT NULL, zulauf int NOT NULL, nachbestellen_ab int NOT NULL, nachbestellen_menge int NOT NULL)");

                    db.ExecuteNonQuery(@"CREATE TABLE [produkteex] ( id uniqueidentifier NOT NULL, bezeichnung nvarchar(100) NOT NULL, bestand int NOT NULL, verfuegbar int NOT NULL)");
                }
            }
            catch
            {
                // Jaja, ist eine Demo ;)
            }
        }

        [SetUp]
        public void TestCaseSetup()
        {
            using (var db = new SQLOperations(() => new SqlConnection(TestConnectionString), transactional: false))
            {
                db.ExecuteNonQuery(@"DELETE FROM [kunden]");
                db.ExecuteNonQuery(@"DELETE FROM [produkte]");
                db.ExecuteNonQuery(@"DELETE FROM [lagerposten]");
                db.ExecuteNonQuery(@"DELETE FROM [produkteex]");
            }
        }

        protected static readonly Guid TestLager = Lagerliste.Hamburg.Id;

        protected CqrsGmbH Erzeuge_TestSystem()
        {
            var host = new CqrsHost_SQL(() => new SqlConnection(TestConnectionString));
            return new CqrsGmbH_CQRSAPI(host);
        }

        protected static Guid Neue_KundenId(CqrsGmbH testsystem)
        {
            return Guid.NewGuid();
        }

        protected static Guid Neue_ProduktId(CqrsGmbH testsystem)
        {
            return Guid.NewGuid();
        }

        protected static Guid Neue_AuftragsId(CqrsGmbH testsystem)
        {
            return Guid.NewGuid();
        }

        protected static void KundeErfassen(CqrsGmbH testsystem, Guid id, string name, string anschrift)
        {
            testsystem.Kunden.KundeErfassen(id, name, anschrift);
        }

        protected static List<KundeInfo> Kundenliste(CqrsGmbH testsystem, Func<KundeInfo, bool> predicate)
        {
            return
                testsystem.Kunden.Kundenliste()
                    .Kunden
                    .Where(predicate)
                    .ToList();
        }

        protected static KundeInfo KundeAbrufen(CqrsGmbH testsystem, Guid kundenid)
        {
            return testsystem.Kunden.Kunde(kundenid);
        }

        protected static void AnschriftAendern(CqrsGmbH testsystem, Guid kundenid, string neueAnschrift)
        {
            testsystem.Kunden.AnschriftAendern(kundenid, neueAnschrift);
        }

        protected static List<BestellungInfo> OffeneBestellungen(CqrsGmbH testsystem)
        {
            return testsystem.Bestellwesen.OffeneBestellungen().Bestellungen;
        }

        protected static void AuftragErfassen(CqrsGmbH testsystem, Guid auftrag, Guid kunde, Guid produkt, int menge)
        {
            testsystem.Bestellwesen.AuftragErfassen(auftrag, kunde, produkt, menge);
        }

        protected static void WareneingangVerzeichnen(CqrsGmbH testsystem, Guid produkt)
        {
            testsystem.Warenwirtschaft.WareneingangVerzeichnen(TestLager, produkt);
        }

        protected static void WareNachbestellen(CqrsGmbH testsystem, Guid produkt, int menge)
        {
            testsystem.Warenwirtschaft.Nachbestellen(TestLager, produkt, menge);
        }

        protected static void ProduktEinlisten(CqrsGmbH testsystem, Guid produktid, string bezeichnung)
        {
            testsystem.Warenwirtschaft.Einlisten(produktid, bezeichnung);
        }

        protected static void AuftragAusfuehren(CqrsGmbH testsystem, Guid auftrag)
        {
            testsystem.Bestellwesen.AuftragAusfuehren(auftrag, TestLager);
        }

        protected static WarenkorbInfo WarenkorbAbrufen(CqrsGmbH testsystem, Guid kunde)
        {
            return testsystem.Warenkorb.FuerKunde(kunde);
        }

        protected static void ArtikelZuWarenkorbHinzufuegen(CqrsGmbH testsystem, WarenkorbInfo warenkorb, Guid produkt, int menge)
        {
            testsystem.Warenkorb.FuegeArtikelHinzu(warenkorb.Id, produkt, menge);
        }

        protected static void ArtikelAusWarenkorbEntfernen(CqrsGmbH testsystem, WarenkorbInfo warenkorb)
        {
            testsystem.Warenkorb.EntferneArtikel(warenkorb.Id, warenkorb.Artikel.First().ZeileId);
        }

        protected static ProduktInfo ProduktAbrufen(CqrsGmbH testsystem, Guid id)
        {
            return testsystem.Warenwirtschaft.ProduktAbrufen(id);
        }

        protected static ProduktInfoEx ProduktExAbrufen(CqrsGmbH testsystem, Guid id)
        {
            return testsystem.Warenwirtschaft.ProduktExAbrufen(id);
        }

        protected static LagerbestandInfo LagerbestandAbrufen(CqrsGmbH testsystem, Guid produkt)
        {
            return testsystem.Warenwirtschaft.LagerbestandAbrufen(TestLager, produkt);
        }

        protected static List<ProduktInfo> ProduktlisteAbrufen(CqrsGmbH testsystem)
        {
            return testsystem.Warenwirtschaft.Produktliste().Produkte;
        }

        protected static void MindestverfuegbarkeitDefinieren(CqrsGmbH testsystem, Guid produkt, int mindestverfuegbarkeit,
            int mindestbestellmenge)
        {
            testsystem.Warenwirtschaft.MindestVerfuegbarkeitDefinieren(TestLager, produkt, mindestverfuegbarkeit, mindestbestellmenge);
        }
    }
}