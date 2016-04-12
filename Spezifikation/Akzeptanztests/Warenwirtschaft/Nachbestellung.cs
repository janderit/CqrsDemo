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

            var produkt = ProduktAbrufen(testsystem, id);
            produkt.LagerBestand.Should().Be(0);
            produkt.MengeImZulauf.Should().Be(20);
            produkt.Verfuegbar.Should().Be(20);
            produkt.Nachbestellt.Should().Be(true);
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

        [Test]
        public void Eine_Nachbestellung_ist_nur_fuer_gelistete_Produkte_moeglich()
        {
            var testsystem = Erzeuge_TestSystem();
            var id = Neue_ProduktId(testsystem);

            Action action = () => WareNachbestellen(testsystem, id, 20);

            action.ShouldThrow<NichtGefunden>();
        }
    }
}