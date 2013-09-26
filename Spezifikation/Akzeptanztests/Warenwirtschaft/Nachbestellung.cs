using System;
using FluentAssertions;
using Infrastruktur.Common;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Nachbestellung : Akzeptanztest
    {

        [Test]
        public void Eine_Nachbestellung_wird_als_Menge_im_Zulauf_registriert()
        {
            var api = TestInstanz();
            var id = api.Warenwirtschaft.Einlisten("Testprodukt");
            api.Warenwirtschaft.Nachbestellen(id, 20);
            var produkt = api.Warenwirtschaft.Produkt(id);

            produkt.LagerBestand.Should().Be(0);
            produkt.MengeImZulauf.Should().Be(20);
            produkt.Verfuegbar.Should().Be(20);
            produkt.Nachbestellt.Should().Be(true);
        }

        [Test]
        public void Eine_Nachbestellung_erfordert_Mengenangabe()
        {
            var api = TestInstanz();
            var id = api.Warenwirtschaft.Einlisten("Testprodukt");
            Action action = () => api.Warenwirtschaft.Nachbestellen(id, 0);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

        [Test]
        public void Eine_Nachbestellung_ist_nur_fuer_gelistete_Produkte_moeglich()
        {
            var api = TestInstanz();
            Action action = () => api.Warenwirtschaft.Nachbestellen(Guid.NewGuid(), 20);
            action.ShouldThrow<NichtGefunden>();
        }
    }
}