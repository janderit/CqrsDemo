using System;
using Resourcen.Warenwirtschaft;

namespace Api.Warenwirtschaft
{
    public interface WarenwirtschaftApi
    {
        Produktliste Produktliste();
        ProduktlisteEx ProduktlisteEx();
        ProduktInfo ProduktAbrufen(Guid produkt);
        ProduktInfoEx ProduktExAbrufen(Guid produkt);
        Lagerbestandsliste LagerbestandslisteAbrufen(Guid lager);
        LagerbestandInfo LagerbestandAbrufen(Guid lager, Guid produkt);

        void Einlisten(Guid produkt, string bezeichnung);

        void Nachbestellen(Guid lagerId, Guid produkt, int menge);
        void WareneingangVerzeichnen(Guid lagerId, Guid produkt);

        void MindestVerfuegbarkeitDefinieren(Guid lagerId, Guid produkt, int mindestverfuegbarkeit, int mindestbestellmenge);
        void AutomatischeNachbestellungenDeaktivieren(Guid lagerId, Guid produkt);
    }
}
