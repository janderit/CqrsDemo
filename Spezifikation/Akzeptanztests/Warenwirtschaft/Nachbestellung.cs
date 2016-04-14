using System;
using FluentAssertions;
using Infrastruktur.Common;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Nachbestellung : Spezifikation
    {

        [Test]
        public void Eine_Nachbestellung_wird_als_Menge_im_Zulauf_registriert()
        {
            var testsystem = Erzeuge_TestSystem();
            var id = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, id, "Testprodukt");

            WareNachbestellen(testsystem, id, 20);

            var lagerbestand = LagerbestandAbrufen(testsystem, id);
            lagerbestand.LagerBestand.Should().Be(0);
            lagerbestand.MengeImZulauf.Should().Be(20);
            lagerbestand.Nachbestellt.Should().Be(true);

            ProduktExAbrufen(testsystem, id).Verfuegbar.Should().Be(20);
        }

        [Test]
        public void Eine_Nachbestellung_erfordert_Mengenangabe()
        {
            var testsystem = Erzeuge_TestSystem();
            var id = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, id, "Testprodukt");

            Action action = () => WareNachbestellen(testsystem, id, 0);

            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }
    }
}