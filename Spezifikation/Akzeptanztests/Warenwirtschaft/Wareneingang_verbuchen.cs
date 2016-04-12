using System;
using FluentAssertions;
using Infrastruktur.Common;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests.Warenwirtschaft
{
    [TestFixture]
    public class Wareneingang_verbuchen : Spezifikation
    {

        [Test]
        public void Ein_Wareneingang_erhoeht_den_Bestand()
        {
            var testsystem = Erzeuge_TestSystem();
            var produktid = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, produktid, "Testprodukt");
            var menge = 20;
            WareNachbestellen(testsystem, produktid, menge);

            WareneingangVerzeichnen(testsystem, produktid);

            var produkt = ProduktAbrufen(testsystem, produktid);
            produkt.LagerBestand.Should().Be(menge);
            produkt.Verfuegbar.Should().Be(menge);
        }

        [Test]
        public void Nach_Wareneingang_ist_die_Menge_im_Zulauf_null()
        {
            var testsystem = Erzeuge_TestSystem();
            var produktid = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, produktid, "Testprodukt");
            var menge = 20;
            WareNachbestellen(testsystem, produktid, menge);

            WareneingangVerzeichnen(testsystem, produktid);

            var produkt = ProduktAbrufen(testsystem, produktid);
            produkt.MengeImZulauf.Should().Be(0);
            produkt.Nachbestellt.Should().Be(false);
        }

        [Test]
        public void Ein_Wareneingang_ohne_Nachbestellung_wird_nicht_akzeptiert()
        {
            var testsystem = Erzeuge_TestSystem();
            var produktid = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, produktid, "Testprodukt");

            Action action = () => WareneingangVerzeichnen(testsystem, produktid);

            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

        [Test]
        public void Doppelter_Wareneingang_zu_Bestellung_schlaegt_fehl()
        {
            var testsystem = Erzeuge_TestSystem();
            var produkt = Neue_ProduktId(testsystem);
            ProduktEinlisten(testsystem, produkt, "Produkt");
            WareNachbestellen(testsystem, produkt, 10);
            WareneingangVerzeichnen(testsystem, produkt);

            Action action = () => WareneingangVerzeichnen(testsystem, produkt);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }
    }
}