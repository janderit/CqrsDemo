using System;
using Infrastruktur.Common;
using Modell_shared;
using Nancy.Responses;

namespace Frontend.Warenwirtschaft
{
    public class Warenwirtschaftsmodul : CqrsGmbH_Web
    {
        public Warenwirtschaftsmodul()
            : base("/warenwirtschaft")
        {
            Get["/"] = parameters => View["produktliste", new {Produktliste = Api().Warenwirtschaft.ProduktlisteEx() , Lagerliste = Lagerliste.Alle}];

            Get["/einlisten"] = parameters => View["einlisten", new {  }];
            Post["/einlisten"] = parameters =>
                                     {
                                         try
                                         {
                                             var produkt = Guid.NewGuid();
                                             Api().Warenwirtschaft.Einlisten(produkt, Request.Form.bezeichnung);
                                             return new RedirectResponse("/warenwirtschaft/");
                                         }
                                         catch (VorgangNichtAusgefuehrt ex)
                                         {
                                             return View["fehler", ex.Message];
                                         }
                                     };

            Get["/{lager}"] = parameters => View["lagerbestand", Api().Warenwirtschaft.LagerbestandslisteAbrufen(parameters.lager)];

            Get["/{lager}/{id}/bestellen"] = parameters => View["nachbestellung", new { }];
            Post["/{lager}/{id}/bestellen"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.Nachbestellen(parameters.lager, parameters.id, Request.Form.menge);
                    return new RedirectResponse("/warenwirtschaft/" + parameters.lager + "/");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

            Get["/{lager}/{id}/wareneingang"] = parameters => View["wareneingang", new { }];
            Post["/{lager}/{id}/wareneingang"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.WareneingangVerzeichnen(parameters.lager, parameters.id);
                    return new RedirectResponse("/warenwirtschaft/" + parameters.lager + "/");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

            Get["/{lager}/{id}/automatikan"] = parameters => View["automatischenachbestellungen", new { }];
            Post["/{lager}/{id}/automatikan"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.MindestVerfuegbarkeitDefinieren(parameters.lager, parameters.id, Request.Form.mindestverfuegbarkeit, Request.Form.mindestbestellmenge);
                    return new RedirectResponse("/warenwirtschaft/" + parameters.lager + "/");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

            Get["/{lager}/{id}/automatikaus"] = parameters => View["automatikaus", new { }];
            Post["/{lager}/{id}/automatikaus"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.AutomatischeNachbestellungenDeaktivieren(parameters.lager, parameters.id);
                    return new RedirectResponse("/warenwirtschaft/" + parameters.lager + "/");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

        }

    }
}