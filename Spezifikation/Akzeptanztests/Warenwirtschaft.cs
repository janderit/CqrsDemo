using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Infrastruktur.Common;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests
{
    [TestFixture]
    public class Warenwirtschaft : Akzeptanztest
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
            Action action = ()=>api.Warenwirtschaft.Nachbestellen(id, 0);
            action.ShouldThrow<VorgangNichtAusgefuehrt>();
        }

        [Test]
        public void Eine_Nachbestellung_ist_nur_fuer_gelistete_Produkte_moeglich()
        {
            var api = TestInstanz();
            Action action = () => api.Warenwirtschaft.Nachbestellen(Guid.NewGuid(), 20);
            action.ShouldThrow<NichtGefunden>();
        }

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


        [Test]
        public void Eine_Auftragsannahme_verringert_die_Verfuegbarkeit_des_Produkts()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Warenwirtschaft.Produkt(produkt).Verfuegbar.Should().Be(13);
        }

        [Test]
        public void Eine_Auftragsannahme_veraendert_die_Lagerbestand_des_Produkts_nicht()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Warenwirtschaft.Produkt(produkt).LagerBestand.Should().Be(20);
        }

        [Test]
        public void Eine_Auftragsausfuehrung_verringert_den_Lagerbestand_des_Produkts()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Bestellwesen.AuftragAusfuehren(auftrag);
            api.Warenwirtschaft.Produkt(produkt).LagerBestand.Should().Be(13);
        }

        [Test]
        public void Eine_Auftragsausfuehrung_veraendert_die_Verfuegbarkeit_des_Produkts_nicht()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 20);
            api.Warenwirtschaft.Wareneingang(produkt);
            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 7, produkt);
            api.Bestellwesen.AuftragAusfuehren(auftrag);
            api.Warenwirtschaft.Produkt(produkt).Verfuegbar.Should().Be(13);
        }

        [Test]
        public void Sinkt_die_Verfuegbarkeit_unter_eine_Mindestgrenze_so_wird_eine_Nachbestellung_ausgeloest()
        {
            var api = TestInstanz();
            var kunde = api.Kunden.KundeErfassen("Testkunde", "Anschrift");
            var produkt = api.Warenwirtschaft.Einlisten("Produkt");
            api.Warenwirtschaft.Nachbestellen(produkt, 200);
            api.Warenwirtschaft.Wareneingang(produkt);

            api.Warenwirtschaft.MindestVerfuegbarkeitDefinieren(produkt, 150, 75);

            var auftrag = api.Bestellwesen.AuftragErfassen(kunde, 100, produkt);

            api.Warenwirtschaft.Produkt(produkt).Verfuegbar.Should().Be(175);
        }


    }
}
