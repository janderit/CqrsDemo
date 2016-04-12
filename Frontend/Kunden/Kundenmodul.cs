using System;
using Infrastruktur.Common;
using Nancy.Responses;

namespace Frontend.Kunden
{
    public class Kundenmodul : CqrsGmbH_Web
    {
        public Kundenmodul()
            : base("/kunden")
        {
            Get["/"] = parameters => View["kundenliste", Api().Kunden.Kundenliste()];

            Get["/erfassen"] = parameters => View["kundeerfassen"];
            Post["/erfassen"] = parameters =>
                                    {
                                        try
                                        {
                                            var id = Guid.NewGuid();
                                            Api().Kunden.KundeErfassen(id, Request.Form.name, Request.Form.anschrift);
                                            return new RedirectResponse("/kunden/");
                                        }
                                        catch (VorgangNichtAusgefuehrt ex)
                                        {
                                            return View["fehler", ex.Message];
                                        }
                                    };

            Get["/{id}/anschriftaendern"] = parameters => View["anschriftaendern", Api().Kunden.Kunde(parameters.id)];
            Post["/{id}/anschriftaendern"] = parameters =>
                                                 {
                                                     try
                                                     {
                                                         Api()
                                                             .Kunden.AnschriftAendern(parameters.id,
                                                                                      Request.Form.neueanschrift);
                                                         return new RedirectResponse("/kunden/");
                                                     }
                                                     catch (VorgangNichtAusgefuehrt ex)
                                                     {
                                                         return View["fehler", ex.Message];
                                                     }
                                                 };
        }

    }
}