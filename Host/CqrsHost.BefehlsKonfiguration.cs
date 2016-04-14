using System;
using Api.Bestellwesen.Aktionen;
using Api.Kunden.Aktionen;
using Api.Warenkorb.Aktionen;
using Api.Warenwirtschaft.Aktionen;
using Infrastruktur.EventSourcing;
using Infrastruktur.Messaging;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Shop;
using Modell.Warenwirtschaft;

namespace Host
{
	partial class CqrsHost
	{

		private void Handle(CommandEnvelope commandEnvelope, KundeErfassen aktion, UnitOfWork unitOfWork)
		{
            var kunde = new KundeRepository(unitOfWork).Retrieve(aktion.KundenId);
		    var warenkorb = new WarenkorbRepository(unitOfWork).Retrieve(Guid.NewGuid());
		    kunde.Erfassen(aktion.Name, aktion.Anschrift, warenkorb);
		}

		private void Handle(CommandEnvelope commandEnvelope, AnschriftAendern aktion, UnitOfWork unitOfWork)
		{
            var kunde = new KundeRepository(unitOfWork).Retrieve(aktion.KundenId);
			kunde.AnschriftAendern(aktion.NeueAnschrift);
			unitOfWork.Commit();
		}

		private void Handle(CommandEnvelope commandEnvelope, AuftragErfassen aktion, UnitOfWork unitOfWork)
		{
		    var auftrag = new AuftragRepository(unitOfWork).Retrieve(aktion.AuftragsId);
            var kunde = new KundeRepository(unitOfWork).Retrieve(aktion.Kunde);

			auftrag.Erfassen(aktion.AuftragsId, aktion.Produkt, aktion.Menge, kunde);
		}

		private void Handle(CommandEnvelope commandEnvelope, AuftragAusfuehren aktion, UnitOfWork unitOfWork)
		{
            var auftrag = new AuftragRepository(unitOfWork).Retrieve(aktion.AuftragId);
            var produkt = new LagerRepository(unitOfWork).Retrieve(aktion.LagerId, auftrag.Produkt);

			auftrag.Ausfuehren(produkt);
		}


		private void Handle(CommandEnvelope commandEnvelope, ProduktEinlisten aktion, UnitOfWork unitOfWork)
		{
            var produkt = new ProduktRepository(unitOfWork).Retrieve(aktion.ProduktId);
			produkt.Einlisten(aktion.Bezeichnung);
		}

		private void Handle(CommandEnvelope commandEnvelope, NachbestellungBeauftragen aktion, UnitOfWork unitOfWork)
		{
            var produkt = new LagerRepository(unitOfWork).Retrieve(aktion.LagerId, aktion.ProduktId);
			produkt.Nachbestellen(aktion.BestellteMenge);
		}

		private void Handle(CommandEnvelope commandEnvelope, WareneingangVerbuchen aktion, UnitOfWork unitOfWork)
		{
            var produkt = new LagerRepository(unitOfWork).Retrieve(aktion.LagerId, aktion.ProduktId);
			produkt.Wareneingang();
		}

		private void Handle(CommandEnvelope commandEnvelope, MindestVerfuegbarkeitDefinieren aktion, UnitOfWork unitOfWork)
		{
            var produkt = new LagerRepository(unitOfWork).Retrieve(aktion.LagerId, aktion.ProduktId);
			produkt.MindestVerfuegbarkeitDefinieren(aktion.MindestVerfuegbarkeit, aktion.MindestBestellmenge);
		}

		private void Handle(CommandEnvelope commandEnvelope, AutomatischeNachbestellungenDeaktivieren aktion, UnitOfWork unitOfWork)
		{
            var produkt = new LagerRepository(unitOfWork).Retrieve(aktion.LagerId, aktion.ProduktId);
			produkt.AutomatischeNachbestellungenDeaktivieren();
		}

        private void Handle(CommandEnvelope commandEnvelope, ArtikelZuWarenkorbHinzufuegen aktion, UnitOfWork unitOfWork)
        {
            var warenkorb = new WarenkorbRepository(unitOfWork).Retrieve(aktion.Warenkorb);
            warenkorb.FuegeHinzu(aktion.Produkt, aktion.Menge);
        }

        private void Handle(CommandEnvelope commandEnvelope, ArtikelAusWarenkorbEntfernen aktion, UnitOfWork unitOfWork)
        {
            var warenkorb = new WarenkorbRepository(unitOfWork).Retrieve(aktion.Warenkorb);
            warenkorb.Entfernen(aktion.Zeile);
        }

        private void Handle(CommandEnvelope commandEnvelope, WarenkorbLeeren aktion, UnitOfWork unitOfWork)
        {
            var warenkorb = new WarenkorbRepository(unitOfWork).Retrieve(aktion.Warenkorb);
            warenkorb.Leeren();
        }

        private void Handle(CommandEnvelope commandEnvelope, WarenkorbBestellen aktion, UnitOfWork unitOfWork)
        {
            var warenkorb = new WarenkorbRepository(unitOfWork).Retrieve(aktion.Warenkorb);
            var auftrags_repo = new AuftragRepository(unitOfWork);
            var produkt_repo = new ProduktRepository(unitOfWork);
            var kunde_repo = new KundeRepository(unitOfWork);
            warenkorb.Bestellen((produkt, menge, kunde) =>
                {
                    var id = Guid.NewGuid();
                    var auftrag = auftrags_repo.Retrieve(id);
                    auftrag.Erfassen(id, produkt, menge, kunde_repo.Retrieve(kunde));
                });
        }

	}
}
