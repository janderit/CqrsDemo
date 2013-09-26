using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenkorb
{
    [TestFixture]
    public class Shopbesuch : Akzeptanztest
    {
         
        [Test]
        public void Ein_neuer_Warenkorb_ist_leer()
        {
            var api = TestInstanz();
            var id = api.Kunden.KundeErfassen("TestName", "TestAnschrift");
            var warenkorb = api.Warenkorb.FuerKunde(id);
            warenkorb.Should().NotBeNull();
            warenkorb.Leer.Should().BeTrue();
        }

        [Test]
        public void Kunde_fuegt_Produkt_zu_Warenkorb_hinzu()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("TestName", "TestAnschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);

            api.Warenkorb.Fuege_Artikel_hinzu(kunde, produkt, 1);

            var warenkorb = api.Warenkorb.FuerKunde(kunde);
            warenkorb.Leer.Should().BeFalse();
        }

    }
}