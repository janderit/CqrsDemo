using System;
using System.Linq;
using FluentAssertions;
using Infrastruktur.Common;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests
{
    [TestFixture]
    public class Bestellwesen : Spezifikation
    {
        [Test]
        public void Ein_Auftrag_wird_als_offene_Bestellung_gefuehrt()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 20);

            var menge = 3;
            var auftrag = Neue_AuftragsId(testsystem);
            AuftragErfassen(testsystem, auftrag, kunde, produkt, menge);
            var bestellungen = OffeneBestellungen(testsystem);
            bestellungen.Should().NotBeNull();
            bestellungen.Should().HaveCount(1);

            var bestellung = bestellungen.Single();
            bestellung.Id.Should().Be(auftrag);
            bestellung.Kunde.Should().Be(kunde);
            bestellung.Produkt.Should().Be(produkt);
            bestellung.Menge.Should().Be(menge);
            bestellung.Erfuellt.Should().Be(false);
        }

        [Test]
        public void Die_Bestellung_enthaelt_Kunden_und_Produktnamen_als_Text()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 20);

            var auftrag = Neue_AuftragsId(testsystem);
            AuftragErfassen(testsystem, auftrag, kunde, produkt, 3);
            var bestellung = OffeneBestellungen(testsystem).Single();
            bestellung.Kundenname.Should().Be("Testkunde");
            bestellung.Produktname.Should().Be("Produkt");
        }

        [Test]
        public void Nach_der_Disposition_ist_der_Auftrag_erfuellt()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 20);

            var auftrag = Neue_AuftragsId(testsystem);
            AuftragErfassen(testsystem, auftrag, kunde, produkt, 3);
            AuftragAusfuehren(testsystem, auftrag);
            var bestellungen = OffeneBestellungen(testsystem);
            bestellungen.Should().BeEmpty();
        }

        [Test]
        public void Eine_Bestellung_erfordert_eine_positive_Mengenangabe()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 20);

            Action action = () => AuftragErfassen(testsystem, Neue_AuftragsId(testsystem), kunde, produkt, 0);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

        [Test]
        public void Eine_Bestellung_erfordert_die_Angabe_des_Produkts()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");

            var produkt = Neue_ProduktId(testsystem);
            Action action = () => AuftragErfassen(testsystem, Neue_AuftragsId(testsystem), kunde, produkt, 20);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

        [Test]
        public void Eine_Bestellung_erfordert_die_Angabe_des_Kunden()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = Neue_KundenId(testsystem);
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 20);

            Action action = () => AuftragErfassen(testsystem, Neue_AuftragsId(testsystem), kunde, produkt, 20);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }


        [Test]
        public void Eine_Auftragsannahme_ist_bei_unzureichender_Verfuegbarkeit_des_Produkts_nicht_moeglich()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var lagerbestand = 5;
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", lagerbestand);

            Action action = () => AuftragErfassen(testsystem, Neue_AuftragsId(testsystem), kunde, produkt, 1 + lagerbestand);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }


        [Test]
        public void Eine_Auftragsausfuehrung_ist_bei_unzureichendem_Lagerbestand_des_Produkts_nicht_moeglich()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var lagerbestand = 5;
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", lagerbestand);
            //WareneingangVerzeichnen(testsystem, produkt);
            WareNachbestellen(testsystem, produkt, 3);

            var auftrag = Neue_AuftragsId(testsystem);
            AuftragErfassen(testsystem, auftrag, kunde, produkt, 1 + lagerbestand);
            Action action = () => AuftragAusfuehren(testsystem, auftrag);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }
    }
}
