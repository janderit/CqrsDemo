using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Automatische_Nachbestellung : Spezifikation
    {

        [Test]
        public void Sinkt_die_Verfuegbarkeit_unter_eine_Mindestgrenze_so_wird_eine_Nachbestellung_ausgeloest()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", 200);

            var mindestverfuegbarkeit = 150;
            var mindestbestellmenge = 75;
            MindestverfuegbarkeitDefinieren(testsystem, produkt, mindestverfuegbarkeit, mindestbestellmenge);

            var auftrag = Neue_AuftragsId(testsystem);
            AuftragErfassen(testsystem, auftrag, kunde, produkt, 100);
            AuftragAusfuehren(testsystem, auftrag);

            ProduktExAbrufen(testsystem, produkt).Verfuegbar.Should().Be(175);
        }
    }
}