using System;
using Api;

namespace Spezifikation
{
    // Schalterklasse, um das System zwischen CQRS_SQL und CQRS_EventSourcing umzustellen
    public class Spezifikation : Spezifikation_Eventsourcing
    //public class Spezifikation : Spezifikation_CQRS_SQL
    {
        protected static Guid TestKundeEinrichten(CqrsGmbH testsystem, string name, string anschrift)
        {
            var kunde = Neue_KundenId(testsystem);

            KundeErfassen(testsystem, kunde, name, anschrift);

            return kunde;
        }

        protected static Guid TestproduktEinlisten_mit_Lagerbestand(CqrsGmbH testsystem, string bezeichnung, int menge)
        {
            var produktid = Neue_ProduktId(testsystem);

            ProduktEinlisten(testsystem, produktid, bezeichnung);
            WareNachbestellen(testsystem, produktid, menge);
            WareneingangVerzeichnen(testsystem, produktid);

            return produktid;
        }
    }
}