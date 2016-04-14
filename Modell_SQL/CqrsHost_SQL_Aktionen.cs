using System;
using System.Linq;
using Api.Bestellwesen.Aktionen;
using Api.Kunden.Aktionen;
using Api.Warenkorb.Aktionen;
using Api.Warenwirtschaft.Aktionen;
using Infrastruktur.Common;
using Infrastruktur.Messaging;

namespace Modell
{
    partial class CqrsHost_SQL
    {
        private void Handle(CommandEnvelope commandEnvelope, KundeErfassen aktion, UnitOfWork unitOfWork)
        {
            var repo = new KundenRepository(unitOfWork);
            repo.Insert(aktion.KundenId, aktion.Name, aktion.Anschrift);
        }

        private void Handle(CommandEnvelope commandEnvelope, AnschriftAendern aktion, UnitOfWork unitOfWork)
        {
            var repo = new KundenRepository(unitOfWork);
            var kunde = repo.Retrieve(aktion.KundenId).Kunden.Single();
            if (kunde.Anschrift != aktion.NeueAnschrift)
            {
                kunde.Anschrift = aktion.NeueAnschrift;
                repo.Update(kunde);
            }
        }

        private void Handle(CommandEnvelope commandEnvelope, AuftragErfassen aktion, UnitOfWork unitOfWork)
        {
        }

        private void Handle(CommandEnvelope commandEnvelope, AuftragAusfuehren aktion, UnitOfWork unitOfWork)
        {
        }


        private void Handle(CommandEnvelope commandEnvelope, ProduktEinlisten aktion, UnitOfWork unitOfWork)
        {
            var repo = new ProduktRepository(unitOfWork);
            repo.Insert(aktion.ProduktId, aktion.Bezeichnung);
        }

        private void Handle(CommandEnvelope commandEnvelope, NachbestellungBeauftragen aktion, UnitOfWork unitOfWork)
        {
            if (aktion.BestellteMenge<=0) throw new VorgangNichtAusgefuehrt("Menge muss angegeben werden");

            var repo = new LagerbestandRepository(unitOfWork);
            var liste = repo.Retrieve(aktion.LagerId, aktion.ProduktId);
            var alt = 0;
            if (liste.Bestand.Count == 0)
            {
                repo.Insert(aktion.LagerId, aktion.ProduktId);
            }
            else
            {
                alt = liste.Bestand.Single().MengeImZulauf;
            }

            repo.Set_Zulauf(aktion.LagerId, aktion.ProduktId, alt + aktion.BestellteMenge);
        }

        private void Handle(CommandEnvelope commandEnvelope, WareneingangVerbuchen aktion, UnitOfWork unitOfWork)
        {
            var repo = new LagerbestandRepository(unitOfWork);
            var liste = repo.Retrieve(aktion.LagerId, aktion.ProduktId);
            if (liste.Bestand.Count == 0) throw new VorgangNichtAusgefuehrt("Wareneingang ohne Bestellung, das darf nicht sein...");

            var posten = liste.Bestand.Single();
            if (posten.MengeImZulauf == 0) throw new VorgangNichtAusgefuehrt("Wareneingang ohne Bestellung, das darf nicht sein...");

            repo.Set_Zulauf(aktion.LagerId, aktion.ProduktId, 0);
            repo.Set_Bestand(aktion.LagerId, aktion.ProduktId, posten.LagerBestand + posten.MengeImZulauf);
        }

        private void Handle(CommandEnvelope commandEnvelope, MindestVerfuegbarkeitDefinieren aktion, UnitOfWork unitOfWork)
        {
        }

        private void Handle(CommandEnvelope commandEnvelope, AutomatischeNachbestellungenDeaktivieren aktion, UnitOfWork unitOfWork)
        {
        }

        private void Handle(CommandEnvelope commandEnvelope, ArtikelZuWarenkorbHinzufuegen aktion, UnitOfWork unitOfWork)
        {
        }

        private void Handle(CommandEnvelope commandEnvelope, ArtikelAusWarenkorbEntfernen aktion, UnitOfWork unitOfWork)
        {
        }

        private void Handle(CommandEnvelope commandEnvelope, WarenkorbLeeren aktion, UnitOfWork unitOfWork)
        {
        }

        private void Handle(CommandEnvelope commandEnvelope, WarenkorbBestellen aktion, UnitOfWork unitOfWork)
        {
        }
    }
}