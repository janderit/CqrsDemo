using System;
using Api;
using Host;
using Infrastruktur.EventSourcing;
using Modell_shared;
using Nancy;

namespace Frontend
{
    public abstract class CqrsGmbH_Web : NancyModule
    {
        static CqrsGmbH_Web()
        {
            // *************** Hier kann man zwischen Event Sourcing und SQL umstellen. Achtung: nur Event Sourcing ist voll implementiert.

            var store = new InMemoryEventStore();
            var port = new CqrsHost(store);

            // -oder -

            /*
            var ConnectionString = "";
            var port = new CqrsHost_SQL(() => new SqlConnection(ConnectionString));
            */

            _api = new Lazy<CqrsGmbH>( // <-- statt IoC
                () =>
                {
                    var api = new CqrsGmbH_CQRSAPI(port);
                    DemoDaten_erzeugen(api);
                    return api;
                });
        }

        private static readonly Lazy<CqrsGmbH> _api;

        protected CqrsGmbH_Web(string basepath):base(basepath)
        {
        }

        private static void DemoDaten_erzeugen(CqrsGmbH_CQRSAPI api)
        {
            api.Kunden.KundeErfassen(Guid.NewGuid(), "Jens Mustermann", "Musterstrasse 10, 12345 Musterstadt");
            var honig = Guid.NewGuid();
            api.Warenwirtschaft.Einlisten(honig, "Honig");
            api.Warenwirtschaft.Nachbestellen(Lagerliste.Hamburg.Id, honig, 100);
            api.Warenwirtschaft.WareneingangVerzeichnen(Lagerliste.Hamburg.Id, honig);
        }

        protected CqrsGmbH Api()
        {
            return _api.Value;
        }
    }
}
