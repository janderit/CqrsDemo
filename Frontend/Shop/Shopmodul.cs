using System;
using Infrastruktur.Common;
using Nancy.Responses;

namespace Frontend.Shop
{
    public class Shopmodul : CqrsGmbh
    {
        public Shopmodul()
            : base("/shop")
        {
            Get["/{knr}"] =
                parameters =>
                View[
                    "produktliste",
                    new {Kunde = (Guid) parameters.knr, Produkte = Api().Warenwirtschaft.Produktliste().Produkte}];

            Get["/{knr}/cart"] =
                parameters =>
                View["warenkorb", new {Kunde = (Guid) parameters.knr, Warenkorb = Api().Warenkorb.FuerKunde(parameters.knr)}];

            Get["/{knr}/cart/add/{produkt}"] = parameters =>
                {
                    var warenkorb = Api().Warenkorb.FuerKunde(parameters.knr).Id;
                    Api().Warenkorb.FuegeArtikelHinzu(warenkorb, parameters.produkt, 1);
                    return new RedirectResponse("/shop/" + parameters.knr + "/cart");
                };

            Get["/{knr}/cart/remove/{zeile}"] = parameters =>
            {
                var warenkorb = Api().Warenkorb.FuerKunde(parameters.knr).Id;
                Api().Warenkorb.EntferneArtikel(warenkorb, parameters.zeile);
                return new RedirectResponse("/shop/" + parameters.knr + "/cart");
            };

            Get["/{knr}/cart/order"] = parameters =>
            {
                var warenkorb = Api().Warenkorb.FuerKunde(parameters.knr).Id;
                Api().Warenkorb.Bestellen(warenkorb);
                return new RedirectResponse("/shop/" + parameters.knr);
            };

            Get["/{knr}/cart/clear"] = parameters =>
            {
                var warenkorb = Api().Warenkorb.FuerKunde(parameters.knr).Id;
                Api().Warenkorb.Leeren(warenkorb);
                return new RedirectResponse("/shop/" + parameters.knr);
            };

        }

    }
}