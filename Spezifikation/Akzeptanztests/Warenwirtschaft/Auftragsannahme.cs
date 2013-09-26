using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Auftragsannahme : Akzeptanztest
    {

        [Test]
        public void Eine_Auftragsannahme_verringert_die_Verfuegbarkeit_des_Produkts()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Warenwirtschaft.Produkt(produkt).Verfuegbar.Should().Be(13);
        }

        [Test]
        public void Eine_Auftragsannahme_veraendert_die_Lagerbestand_des_Produkts_nicht()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Warenwirtschaft.Produkt(produkt).LagerBestand.Should().Be(20);
        }

    }
}