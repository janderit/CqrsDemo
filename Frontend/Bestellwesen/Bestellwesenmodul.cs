 using System;
using Infrastruktur.Common;
using Nancy.Responses;

namespace Frontend.Bestellwesen
{
    public class Bestellwesenmodul : CqrsGmbH_Web
    {
        public Bestellwesenmodul()
            : base("/bestellwesen")
        {
            Get["/"] = parameters => View["offenebestellungenliste", Api().Bestellwesen.OffeneBestellungen()];

            Get["/erfassen"] = parameters => View["auftragerfassen", new
            {
                Kunden = Api().Kunden.Kundenliste().Kunden,
                Produkte = Api().Warenwirtschaft.Produktliste().Produkte,
                Kunde = Request.Query.kunde
            }];
            Post["/erfassen"] = parameters =>
            {
                try
                {
                    var auftrag = Guid.NewGuid();
                    Api()
                        .Bestellwesen.AuftragErfassen(auftrag, Request.Form.kunde, Request.Form.produkt,
                            Request.Form.menge);
                    return new RedirectResponse("/bestellwesen/");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };


            Get["/ausfuehren/{id}"] = parameters => View["ausfuehren"];
            Post["/ausfuehren/{id}"] = parameters =>
            {
                try
                {
                    Api().Bestellwesen.AuftragAusfuehren(parameters.Id);
                    return new RedirectResponse("/bestellwesen/");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

        }

    }
}