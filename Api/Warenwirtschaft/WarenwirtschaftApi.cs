using System;
using Resourcen.Warenwirtschaft;

namespace Api.Warenwirtschaft
{
    public interface WarenwirtschaftApi
    {
        Produktliste Produktliste();
        ProduktInfo ProduktAbrufen(Guid produkt);

        void Einlisten(Guid produkt, string bezeichnung);

        void Nachbestellen(Guid produkt, int menge);
        void WareneingangVerzeichnen(Guid produkt);

        void MindestVerfuegbarkeitDefinieren(Guid produkt, int mindestverfuegbarkeit, int mindestbestellmenge);
        void AutomatischeNachbestellungenDeaktivieren(Guid produkt);
    }
}
