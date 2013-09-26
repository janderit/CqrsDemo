using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Auftragsausfuehrung : Akzeptanztest
    {

        [Test]
        public void Eine_Auftragsausfuehrung_verringert_den_Lagerbestand_des_Produkts()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Bestellwesen.AuftragAusfuehren(auftrag);
            api.Warenwirtschaft.Produkt(produkt).LagerBestand.Should().Be(13);
        }

        [Test]
        public void Eine_Auftragsausfuehrung_veraendert_die_Verfuegbarkeit_des_Produkts_nicht()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Bestellwesen.AuftragAusfuehren(auftrag);
            api.Warenwirtschaft.Produkt(produkt).Verfuegbar.Should().Be(13);
        }



    }
}