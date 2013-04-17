using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Bestellwesen.Aktionen;
using Api.Kunden.Aktionen;
using Api.Warenwirtschaft.Aktionen;
using Infrastruktur.EventSourcing;
using Infrastruktur.Messaging;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Warenwirtschaft;

namespace Host
{
	partial class CqrsHost
	{
	    
	    private void Handle(Command command, KundeErfassen aktion)
        {
            var repo = new KundeRepository(aktion.KundenId, _eventStore);
            var kunde = repo.Retrieve();
            kunde.Erfassen(aktion.Name, aktion.Anschrift);
	        repo.Commit();
        }

        private void Handle(Command command, AnschriftAendern aktion)
        {
            var repo = new KundeRepository(aktion.KundenId, _eventStore);
            var kunde = repo.Retrieve();
            kunde.AnschriftAendern(aktion.NeueAnschrift);
            repo.Commit();
        }

        private void Handle(Command command, AuftragErfassen aktion)
        {
            var repo = new AuftragRepository(aktion.AuftragsId, _eventStore);
            var auftrag = repo.Retrieve();

            var produktrepo = new ProduktRepository(aktion.Produkt, _eventStore);
            var kunderepo = new KundeRepository(aktion.Kunde, _eventStore);

            auftrag.Erfassen(produktrepo.Retrieve(), aktion.Menge, kunderepo.Retrieve());

            produktrepo.Commit();
            repo.Commit();
        }

        private void Handle(Command command, AuftragAusfuehren aktion)
        {
            var repo = new AuftragRepository(aktion.AuftragId, _eventStore);
            var auftrag = repo.Retrieve();

            var produktrepo = new ProduktRepository(auftrag.Produkt, _eventStore);
            auftrag.Ausfuehren(produktrepo.Retrieve());

            produktrepo.Commit();
            repo.Commit();
        }


        private void Handle(Command command, ProduktEinlisten aktion)
        {
            var repo = new ProduktRepository(aktion.ProduktId, _eventStore);
            var produkt = repo.Retrieve();
            produkt.Einlisten(aktion.Bezeichnung);
            repo.Commit();
        }

        private void Handle(Command command, NachbestellungBeauftragen aktion)
        {
            var repo = new ProduktRepository(aktion.ProduktId, _eventStore);
            var produkt = repo.Retrieve();
            produkt.Nachbestellen(aktion.BestellteMenge);
            repo.Commit();
        }

        private void Handle(Command command, WareneingangVerbuchen aktion)
        {
            var repo = new ProduktRepository(aktion.ProduktId, _eventStore);
            var produkt = repo.Retrieve();
            produkt.Wareneingang();
            repo.Commit();
        }

        private void Handle(Command command, MindestVerfuegbarkeitDefinieren aktion)
        {
            var repo = new ProduktRepository(aktion.ProduktId, _eventStore);
            var produkt = repo.Retrieve();
            produkt.MindestVerfuegbarkeitDefinieren(aktion.MindestVerfuegbarkeit, aktion.MindestBestellmenge);
            repo.Commit();
        }

        private void Handle(Command command, AutomatischeNachbestellungenDeaktivieren aktion)
        {
            var repo = new ProduktRepository(aktion.ProduktId, _eventStore);
            var produkt = repo.Retrieve();
            produkt.AutomatischeNachbestellungenDeaktivieren();
            repo.Commit();
        }

	}
}
