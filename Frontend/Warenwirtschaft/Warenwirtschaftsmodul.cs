using System;
using Infrastruktur.Common;
using Nancy.Responses;

namespace Frontend.Warenwirtschaft
{
    public class Warenwirtschaftsmodul : CqrsGmbh
    {
        public Warenwirtschaftsmodul()
            : base("/warenwirtschaft")
        {
            Get["/"] = parameters => View["produktliste", Api().Warenwirtschaft.Produktliste()];

            Get["/einlisten"] = parameters => View["einlisten", new {  }];
            Post["/einlisten"] = parameters =>
                                     {
                                         try
                                         {
                                             Api().Warenwirtschaft.Einlisten(Request.Form.bezeichnung);
                                             return new RedirectResponse("/warenwirtschaft");
                                         }
                                         catch (VorgangNichtAusgefuehrt ex)
                                         {
                                             return View["fehler", ex.Message];
                                         }
                                     };

            Get["/{id}/bestellen"] = parameters => View["nachbestellung", new { }];
            Post["/{id}/bestellen"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.Nachbestellen(parameters.id, Request.Form.menge);
                    return new RedirectResponse("/warenwirtschaft");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

            Get["/{id}/wareneingang"] = parameters => View["wareneingang", new { }];
            Post["/{id}/wareneingang"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.Wareneingang(parameters.id);
                    return new RedirectResponse("/warenwirtschaft");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

            Get["/{id}/automatikan"] = parameters => View["automatischenachbestellungen", new { }];
            Post["/{id}/automatikan"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.MindestVerfuegbarkeitDefinieren(parameters.id, Request.Form.mindestverfuegbarkeit, Request.Form.mindestbestellmenge);
                    return new RedirectResponse("/warenwirtschaft");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };

            Get["/{id}/automatikaus"] = parameters => View["automatikaus", new { }];
            Post["/{id}/automatikaus"] = parameters =>
            {
                try
                {
                    Api().Warenwirtschaft.AutomatischeNachbestellungenDeaktivieren(parameters.id);
                    return new RedirectResponse("/warenwirtschaft");
                }
                catch (VorgangNichtAusgefuehrt ex)
                {
                    return View["fehler", ex.Message];
                }
            };
            
        }

    }
}