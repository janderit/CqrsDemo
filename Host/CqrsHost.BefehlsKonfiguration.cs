using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Bestellwesen.Aktionen;
using Api.Kunden.Aktionen;
using Api.Warenkorb.Aktionen;
using Api.Warenwirtschaft.Aktionen;
using Infrastruktur.EventSourcing;
using Infrastruktur.Messaging;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Warenkorb;
using Modell.Warenwirtschaft;

namespace Host
{
	partial class CqrsHost
	{

		private void Handle(Command command, KundeErfassen aktion, UnitOfWork unitOfWork)
		{
            var kunde = new KundeRepository(unitOfWork).Retrieve(aktion.KundenId);
		    var warenkorb = new WarenkorbRepository(unitOfWork).Retrieve(Guid.NewGuid());
		    kunde.Erfassen(aktion.Name, aktion.Anschrift, warenkorb);
		}

		private void Handle(Command command, AnschriftAendern aktion, UnitOfWork unitOfWork)
		{
            var kunde = new KundeRepository(unitOfWork).Retrieve(aktion.KundenId);
			kunde.AnschriftAendern(aktion.NeueAnschrift);
			unitOfWork.Commit();
		}

		private void Handle(Command command, AuftragErfassen aktion, UnitOfWork unitOfWork)
		{
		    var auftrag = new AuftragRepository(unitOfWork).Retrieve(aktion.AuftragsId);
            var produkt = new ProduktRepository(unitOfWork).Retrieve(aktion.Produkt);
            var kunde = new KundeRepository(unitOfWork).Retrieve(aktion.Kunde);

			auftrag.Erfassen(aktion.AuftragsId, produkt, aktion.Menge, kunde);
		}

		private void Handle(Command command, AuftragAusfuehren aktion, UnitOfWork unitOfWork)
		{
            var auftrag = new AuftragRepository(unitOfWork).Retrieve(aktion.AuftragId);
            var produkt = new ProduktRepository(unitOfWork).Retrieve(auftrag.Produkt);

			auftrag.Ausfuehren(produkt);
		}


		private void Handle(Command command, ProduktEinlisten aktion, UnitOfWork unitOfWork)
		{
            var produkt = new ProduktRepository(unitOfWork).Retrieve(aktion.ProduktId);
			produkt.Einlisten(aktion.Bezeichnung);
		}

		private void Handle(Command command, NachbestellungBeauftragen aktion, UnitOfWork unitOfWork)
		{
            var produkt = new ProduktRepository(unitOfWork).Retrieve(aktion.ProduktId);
			produkt.Nachbestellen(aktion.BestellteMenge);
		}

		private void Handle(Command command, WareneingangVerbuchen aktion, UnitOfWork unitOfWork)
		{
            var produkt = new ProduktRepository(unitOfWork).Retrieve(aktion.ProduktId);
			produkt.Wareneingang();
		}

		private void Handle(Command command, MindestVerfuegbarkeitDefinieren aktion, UnitOfWork unitOfWork)
		{
            var produkt = new ProduktRepository(unitOfWork).Retrieve(aktion.ProduktId);
			produkt.MindestVerfuegbarkeitDefinieren(aktion.MindestVerfuegbarkeit, aktion.MindestBestellmenge);
		}

		private void Handle(Command command, AutomatischeNachbestellungenDeaktivieren aktion, UnitOfWork unitOfWork)
		{
            var produkt = new ProduktRepository(unitOfWork).Retrieve(aktion.ProduktId);
			produkt.AutomatischeNachbestellungenDeaktivieren();
		}

        private void Handle(Command command, ArtikelZuWarenkorbHinzufuegen aktion, UnitOfWork unitOfWork)
        {
            var repo = new WarenkorbRepository(unitOfWork);
            var warenkorb = repo.Retrieve(aktion.Warenkorb);
            warenkorb.FuegeHinzu(aktion.Produkt, aktion.Menge);
        }

	}
}
