using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Produkt_einlisten : Spezifikation
    {
        [Test]
        public void Ein_eingelistetes_Produkt_ist_in_der_Produktliste_aufgefuehrt()
        {
            var testsystem = Erzeuge_TestSystem();

            var id = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, id, "Testprodukt");

            var produkte = ProduktlisteAbrufen(testsystem);
            produkte.Should().NotBeNull();
            produkte.Should().HaveCount(1);

            var produkt = produkte.Single();
            produkt.Should().NotBeNull();
            produkt.Id.Should().Be(id);
            produkt.Bezeichnung.Should().Be("Testprodukt");

            produkt = ProduktAbrufen(testsystem, id);
            produkt.Should().NotBeNull();
            produkt.Id.Should().Be(id);
            produkt.Bezeichnung.Should().Be("Testprodukt");
        }

        [Test]
        public void Ein_eingelistetes_Produkt_hat_keinen_Bestand_etc()
        {
            var testsystem = Erzeuge_TestSystem();
            var id = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, id, "Testprodukt");

            var lagerbestand = LagerbestandAbrufen(testsystem, id);
            lagerbestand.LagerBestand.Should().Be(0);
            lagerbestand.MengeImZulauf.Should().Be(0);
            lagerbestand.Nachbestellt.Should().Be(false);

            ProduktExAbrufen(testsystem, id).Verfuegbar.Should().Be(0);
        }
    }
}
