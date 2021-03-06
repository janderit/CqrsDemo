using FluentAssertions;
using NUnit.Framework;
using Resourcen.Warenwirtschaft;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Auftragsannahme : Spezifikation
    {

        [Test]
        public void Eine_Auftragsannahme_verringert_die_Verfuegbarkeit_des_Produkts()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 20);

            var auftrag = Neue_AuftragsId(testsystem);
            AuftragErfassen(testsystem, auftrag, kunde, produkt, 7);

            ProduktExAbrufen(testsystem, produkt).Verfuegbar.Should().Be(13);
        }

        [Test]
        public void Eine_Auftragsannahme_veraendert_die_Lagerbestand_des_Produkts_nicht()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 20);

            var auftrag = Neue_AuftragsId(testsystem);
            AuftragErfassen(testsystem, auftrag, kunde, produkt, 7);

            LagerbestandAbrufen(testsystem, produkt).LagerBestand.Should().Be(20);
        }

    }
}