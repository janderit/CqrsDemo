using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api;
using FluentAssertions;
using Infrastruktur.Common;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests
{
    [TestFixture]
    public class Bestellwesen : Akzeptanztest
    {

        [Test]
        public void Ein_Auftrag_wird_als_offene_Bestellung_gefuehrt()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);

            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 3, produkt);
            var bestellungen = api.Bestellwesen.OffeneBestellungen().Bestellungen;
            bestellungen.Should().NotBeNull();
            bestellungen.Should().HaveCount(1);

            var bestellung = bestellungen.Single();
            bestellung.Id.Should().Be(auftrag);
            bestellung.Kunde.Should().Be(kunde);
            bestellung.Produkt.Should().Be(produkt);
            bestellung.Menge.Should().Be(3);
            bestellung.Erfuellt.Should().Be(false);
        }

        [Test]
        public void Die_Bestellung_enthaelt_Kunden_und_Produktnamen_als_Text()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);

            api.Bestellwesen.AuftragErfassen(kunde, 3, produkt);
            var bestellung = api.Bestellwesen.OffeneBestellungen().Bestellungen.Single();
            bestellung.Kundenname.Should().Be("Testkunde");
            bestellung.Produktname.Should().Be("Produkt");
        }

        [Test]
        public void Nach_der_Disposition_ist_der_Auftrag_erfuellt()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 3, produkt);
            api.Bestellwesen.AuftragAusfuehren(auftrag);
            var bestellungen = api.Bestellwesen.OffeneBestellungen().Bestellungen;
            bestellungen.Should().BeEmpty();
        }

        [Test]
        public void Eine_Bestellung_erfordert_eine_positive_Mengenangabe()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            Action action = () => api.Bestellwesen.AuftragErfassen(kunde, 0, produkt);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

        [Test]
        public void Eine_Bestellung_erfordert_die_Angabe_des_Produkts()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = Guid.NewGuid();
            Action action = () => api.Bestellwesen.AuftragErfassen(kunde, 20, produkt);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

        [Test]
        public void Eine_Bestellung_erfordert_die_Angabe_des_Kunden()
        {
            var api = TestInstanz();
            var kunde = Guid.NewGuid();
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            Action action = () => api.Bestellwesen.AuftragErfassen(kunde, 20, produkt);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }


        [Test]
        public void Eine_Auftragsannahme_ist_bei_unzureichender_Verfuegbarkeit_des_Produkts_nicht_moeglich()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 5);
            api.Warenwirtschaft.Wareneingang(produkt);
            Action action = () => api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }


        [Test]
        public void Eine_Auftragsausfuehrung_ist_bei_unzureichendem_Lagerbestand_des_Produkts_nicht_moeglich()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 5);
            api.Warenwirtschaft.Wareneingang(produkt);
            api.Warenwirtschaft.Nachbestellen(produkt, 5);
            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            Action action = () => api.Bestellwesen.AuftragAusfuehren(auftrag);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

    }
}
