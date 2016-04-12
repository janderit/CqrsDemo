using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Shop
{
    [TestFixture]
    public class Shopbesuch : Spezifikation
    {

        [Test]
        public void Ein_neuer_Warenkorb_ist_leer()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");

            // NO ACTION

            var warenkorb = WarenkorbAbrufen(testsystem, kunde);
            warenkorb.Should().NotBeNull();
            warenkorb.Leer.Should().BeTrue();
        }

        [Test]
        public void Kunde_fuegt_Produkt_zu_Warenkorb_hinzu()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt", menge: 20);
            var warenkorb = WarenkorbAbrufen(testsystem, kunde);

            ArtikelZuWarenkorbHinzufuegen(testsystem, warenkorb, produkt, menge: 1);

            warenkorb = WarenkorbAbrufen(testsystem, kunde);
            warenkorb.Leer.Should().BeFalse();
        }

        [Test]
        public void Kunde_entfernt_Produkt_aus_Warenkorb()
        {
            var testsystem = Erzeuge_TestSystem();
            var kunde = TestKundeEinrichten(testsystem, "Testkunde", "Anschrift");
            var produkt1 = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt 1", menge: 20);
            var produkt2 = TestproduktEinlisten_mit_Lagerbestand(testsystem, "Produkt 2", menge: 20);

            var warenkorb = WarenkorbAbrufen(testsystem, kunde);
            ArtikelZuWarenkorbHinzufuegen(testsystem, warenkorb, produkt1, menge: 1);
            ArtikelZuWarenkorbHinzufuegen(testsystem, warenkorb, produkt2, menge: 1);

            warenkorb = WarenkorbAbrufen(testsystem, kunde);
            warenkorb.Artikel.Count.Should().Be(2);

            ArtikelAusWarenkorbEntfernen(testsystem, warenkorb);

            warenkorb = WarenkorbAbrufen(testsystem, kunde);
            warenkorb.Leer.Should().BeFalse();
            warenkorb.Artikel.Count.Should().Be(1);
        }
    }
}