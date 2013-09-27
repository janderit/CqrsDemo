using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Shop
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

            var warenkorb = api.Warenkorb.FuerKunde(kunde);
            api.Warenkorb.FuegeArtikelHinzu(warenkorb.Id, produkt, 1);

            warenkorb = api.Warenkorb.FuerKunde(kunde);
            warenkorb.Leer.Should().BeFalse();
        }

        [Test]
        public void Kunde_entfernt_Produkt_aus_Warenkorb()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("TestName", "TestAnschrift");
            var produkt1 = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt1, 20);
            api.Warenwirtschaft.Wareneingang(produkt1);

            var produkt2 = api.Warenwirtschaft.Einlisten("Produkt 2");
            api.Warenwirtschaft.Nachbestellen(produkt2, 20);
            api.Warenwirtschaft.Wareneingang(produkt2);

            var warenkorb = api.Warenkorb.FuerKunde(kunde);
            api.Warenkorb.FuegeArtikelHinzu(warenkorb.Id, produkt1, 1);
            api.Warenkorb.FuegeArtikelHinzu(warenkorb.Id, produkt2, 1);

            warenkorb = api.Warenkorb.FuerKunde(kunde);
            warenkorb.Artikel.Count.Should().Be(2);

            api.Warenkorb.EntferneArtikel(warenkorb.Id, warenkorb.Artikel.First().ZeileId);

            warenkorb = api.Warenkorb.FuerKunde(kunde);
            warenkorb.Leer.Should().BeFalse();
            warenkorb.Artikel.Count.Should().Be(1);
        }

    }
}