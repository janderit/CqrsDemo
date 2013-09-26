using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Produkt_einlisten : Akzeptanztest
    {
        [Test]
        public void Ein_eingelistetes_Produkt_ist_in_der_Produktliste_aufgefuehrt()
        {
            var api = TestInstanz();
            var id = api.Warenwirtschaft.Einlisten("Testprodukt");

            var produkte = api.Warenwirtschaft.Produktliste().Produkte;
            produkte.Should().NotBeNull();
            produkte.Should().HaveCount(1);

            var produkt = produkte.Single();
            produkt.Should().NotBeNull();
            produkt.Id.Should().Be(id);
            produkt.Bezeichnung.Should().Be("Testprodukt");

            produkt = api.Warenwirtschaft.Produkt(id);
            produkt.Should().NotBeNull();
            produkt.Id.Should().Be(id);
            produkt.Bezeichnung.Should().Be("Testprodukt");
        }

        [Test]
        public void Ein_eingelistetes_Produkt_hat_keinen_Bestand_etc()
        {
            var api = TestInstanz();
            var id = api.Warenwirtschaft.Einlisten("Testprodukt");
            var produkt = api.Warenwirtschaft.Produkt(id);

            produkt.LagerBestand.Should().Be(0);
            produkt.MengeImZulauf.Should().Be(0);
            produkt.Verfuegbar.Should().Be(0);
            produkt.Nachbestellt.Should().Be(false);
        }
    }
}
