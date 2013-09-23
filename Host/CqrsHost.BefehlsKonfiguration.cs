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

		private void Handle(Command command, KundeErfassen aktion, UnitOfWork unitOfWork)
		{	        
			var repo = new KundeRepository(aktion.KundenId, unitOfWork);
			var kunde = repo.Retrieve();
			kunde.Erfassen(aktion.Name, aktion.Anschrift);	        
		}

		private void Handle(Command command, AnschriftAendern aktion, UnitOfWork unitOfWork)
		{
			var repo = new KundeRepository(aktion.KundenId, unitOfWork);
			var kunde = repo.Retrieve();
			kunde.AnschriftAendern(aktion.NeueAnschrift);
			unitOfWork.Commit();
		}

		private void Handle(Command command, AuftragErfassen aktion, UnitOfWork unitOfWork)
		{
			var repo = new AuftragRepository(aktion.AuftragsId, unitOfWork);
			var auftrag = repo.Retrieve();

			var produktrepo = new ProduktRepository(aktion.Produkt, unitOfWork);
			var kunderepo = new KundeRepository(aktion.Kunde, unitOfWork);

			auftrag.Erfassen(produktrepo.Retrieve(), aktion.Menge, kunderepo.Retrieve());
		}

		private void Handle(Command command, AuftragAusfuehren aktion, UnitOfWork unitOfWork)
		{
			var repo = new AuftragRepository(aktion.AuftragId, unitOfWork);
			var auftrag = repo.Retrieve();

			var produktrepo = new ProduktRepository(auftrag.Produkt, unitOfWork);
			auftrag.Ausfuehren(produktrepo.Retrieve());
		}


		private void Handle(Command command, ProduktEinlisten aktion, UnitOfWork unitOfWork)
		{
			var repo = new ProduktRepository(aktion.ProduktId, unitOfWork);
			var produkt = repo.Retrieve();
			produkt.Einlisten(aktion.Bezeichnung);
		}

		private void Handle(Command command, NachbestellungBeauftragen aktion, UnitOfWork unitOfWork)
		{
			var repo = new ProduktRepository(aktion.ProduktId, unitOfWork);
			var produkt = repo.Retrieve();
			produkt.Nachbestellen(aktion.BestellteMenge);
		}

		private void Handle(Command command, WareneingangVerbuchen aktion, UnitOfWork unitOfWork)
		{
			var repo = new ProduktRepository(aktion.ProduktId, unitOfWork);
			var produkt = repo.Retrieve();
			produkt.Wareneingang();
		}

		private void Handle(Command command, MindestVerfuegbarkeitDefinieren aktion, UnitOfWork unitOfWork)
		{
			var repo = new ProduktRepository(aktion.ProduktId, unitOfWork);
			var produkt = repo.Retrieve();
			produkt.MindestVerfuegbarkeitDefinieren(aktion.MindestVerfuegbarkeit, aktion.MindestBestellmenge);
		}

		private void Handle(Command command, AutomatischeNachbestellungenDeaktivieren aktion, UnitOfWork unitOfWork)
		{
			var repo = new ProduktRepository(aktion.ProduktId, unitOfWork);
			var produkt = repo.Retrieve();
			produkt.AutomatischeNachbestellungenDeaktivieren();
		}

	}
}
