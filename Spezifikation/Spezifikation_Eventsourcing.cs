using System;
using System.Collections.Generic;
using System.Linq;
using Api;
using Host;
using Infrastruktur.EventSourcing;
using Resourcen.Bestellwesen;
using Resourcen.Kunden;
using Resourcen.Shop;
using Resourcen.Warenwirtschaft;

namespace Spezifikation
{
    public abstract class Spezifikation_Eventsourcing
    {
        protected CqrsGmbH Erzeuge_TestSystem(Action<EventStore> hook = null)
        {
            var store = new InMemoryEventStore();
            var host = new CqrsHost(store);
            if (hook != null) hook(store);
            return new CqrsGmbH_CQRSAPI(host);
        }

        protected static Guid Neue_KundenId(CqrsGmbH testsystem)
        {
            return Guid.NewGuid();
        }

        protected static Guid Neue_ProduktId(CqrsGmbH testsystem)
        {
            return Guid.NewGuid();
        }

        protected static Guid Neue_AuftragsId(CqrsGmbH testsystem)
        {
            return Guid.NewGuid();
        }

        protected static void KundeErfassen(CqrsGmbH testsystem, Guid id, string name, string anschrift)
        {
            testsystem.Kunden.KundeErfassen(id, name, anschrift);
        }

        protected static List<KundeInfo> Kundenliste(CqrsGmbH testsystem, Func<KundeInfo, bool> predicate)
        {
            return
                testsystem.Kunden.Kundenliste()
                    .Kunden
                    .Where(predicate)
                    .ToList();
        }

        protected static KundeInfo KundeAbrufen(CqrsGmbH testsystem, Guid kundenid)
        {
            return testsystem.Kunden.Kunde(kundenid);
        }

        protected static void AnschriftAendern(CqrsGmbH testsystem, Guid kundenid, string neueAnschrift)
        {
            testsystem.Kunden.AnschriftAendern(kundenid, neueAnschrift);
        }

        protected static List<Bestellung> OffeneBestellungen(CqrsGmbH testsystem)
        {
            return testsystem.Bestellwesen.OffeneBestellungen().Bestellungen;
        }

        protected static void AuftragErfassen(CqrsGmbH testsystem, Guid auftrag, Guid kunde, Guid produkt, int menge)
        {
            testsystem.Bestellwesen.AuftragErfassen(auftrag, kunde, produkt, menge);
        }

        protected static void WareneingangVerzeichnen(CqrsGmbH testsystem, Guid produkt)
        {
            testsystem.Warenwirtschaft.WareneingangVerzeichnen(produkt);
        }

        protected static void WareNachbestellen(CqrsGmbH testsystem, Guid produkt, int menge)
        {
            testsystem.Warenwirtschaft.Nachbestellen(produkt, menge);
        }

        protected static void ProduktEinlisten(CqrsGmbH testsystem, Guid produktid, string bezeichnung)
        {
            testsystem.Warenwirtschaft.Einlisten(produktid, bezeichnung);
        }

        protected static void AuftragAusfuehren(CqrsGmbH testsystem, Guid auftrag)
        {
            testsystem.Bestellwesen.AuftragAusfuehren(auftrag);
        }

        protected static Warenkorb WarenkorbAbrufen(CqrsGmbH testsystem, Guid kunde)
        {
            return testsystem.Warenkorb.FuerKunde(kunde);
        }

        protected static void ArtikelZuWarenkorbHinzufuegen(CqrsGmbH testsystem, Warenkorb warenkorb, Guid produkt, int menge)
        {
            testsystem.Warenkorb.FuegeArtikelHinzu(warenkorb.Id, produkt, menge);
        }

        protected static void ArtikelAusWarenkorbEntfernen(CqrsGmbH testsystem, Warenkorb warenkorb)
        {
            testsystem.Warenkorb.EntferneArtikel(warenkorb.Id, warenkorb.Artikel.First().ZeileId);
        }

        protected static ProduktInfo ProduktAbrufen(CqrsGmbH testsystem, Guid id)
        {
            return testsystem.Warenwirtschaft.ProduktAbrufen(id);
        }

        protected static List<ProduktInfo> ProduktlisteAbrufen(CqrsGmbH testsystem)
        {
            return testsystem.Warenwirtschaft.Produktliste().Produkte;
        }

        protected static void MindestverfuegbarkeitDefinieren(CqrsGmbH testsystem, Guid produkt, int mindestverfuegbarkeit,
            int mindestbestellmenge)
        {
            testsystem.Warenwirtschaft.MindestVerfuegbarkeitDefinieren(produkt, mindestverfuegbarkeit, mindestbestellmenge);
        }
    }
}
