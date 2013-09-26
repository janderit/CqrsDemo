using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Automatische_Nachbestellung : Akzeptanztest
    {

        [Test]
        public void Sinkt_die_Verfuegbarkeit_unter_eine_Mindestgrenze_so_wird_eine_Nachbestellung_ausgeloest()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 200);
            api.Warenwirtschaft.Wareneingang(produkt);

            api.Warenwirtschaft.MindestVerfuegbarkeitDefinieren(produkt, 150, 75);

            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 100, produkt);

            api.Warenwirtschaft.Produkt(produkt).Verfuegbar.Should().Be(175);
        }


    }
}