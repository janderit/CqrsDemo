using System;
using FluentAssertions;
using Infrastruktur.Common;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Wareneingang_verbuchen : Akzeptanztest
    {

        [Test]
        public void Ein_Wareneingang_erhoeht_den_Bestand()
        {
            var api = TestInstanz();
            var id = api.Warenwirtschaft.Einlisten("Testprodukt");
            api.Warenwirtschaft.Nachbestellen(id, 20);
            api.Warenwirtschaft.Wareneingang(id);

            var produkt = api.Warenwirtschaft.Produkt(id);

            produkt.LagerBestand.Should().Be(20);
            produkt.Verfuegbar.Should().Be(20);
        }

        [Test]
        public void Nach_Wareneingang_ist_die_Menge_im_Zulauf_null()
        {
            var api = TestInstanz();
            var id = api.Warenwirtschaft.Einlisten("Testprodukt");
            api.Warenwirtschaft.Nachbestellen(id, 20);
            api.Warenwirtschaft.Wareneingang(id);

            var produkt = api.Warenwirtschaft.Produkt(id);

            produkt.MengeImZulauf.Should().Be(0);
            produkt.Nachbestellt.Should().Be(false);
        }

        [Test]
        public void Ein_Wareneingang_ohne_Nachbestellung_wird_nicht_akzeptiert()
        {
            var api = TestInstanz();
            var id = api.Warenwirtschaft.Einlisten("Testprodukt");
            Action action = () => api.Warenwirtschaft.Wareneingang(id);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }
    }
}