using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Api.Bestellwesen.Abfragen;
using Api.Kunden.Abfragen;
using Api.Meta;
using Api.Warenkorb.Abfragen;
using Api.Warenwirtschaft.Abfragen;
using Infrastruktur.Common;
using Infrastruktur.Messaging;
using Modell.Bestellwesen;
using Modell.Kunden;
using Modell.Warenwirtschaft;
using Readmodels;
using Resourcen.Bestellwesen;
using Resourcen.Kunden;
using Resourcen.Meta;
using Resourcen.Shop;
using Resourcen.Warenwirtschaft;

namespace Host
{
	partial class CqrsHost
	{

		private Kundenliste Handle(QueryEnvelope queryEnvelope, KundenlisteAbfrage abfrage)
		{
			return new Kundenliste
					   {
						   Kunden = KundenProjektion.AlleIDs(_eventStore.History)
						   .Select(_kunden.Access).ToList()
					   };
		}

        private Produktliste Handle(QueryEnvelope queryEnvelope, ProduktlisteAbfrage abfrage)
        {
            return new Produktliste { Produkte = AlleProduktInfos() };
        }

	    private List<ProduktInfo> AlleProduktInfos()
	    {
	        var alle_produkt_ids = ProduktProjektion.AlleIDs(_eventStore.History);
	        var produkte = alle_produkt_ids.Select(_ => _produkte.Access(_)).ToList();
	        return produkte;
	    }

	    private ProduktlisteEx Handle(QueryEnvelope queryEnvelope, ProduktlisteExAbfrage abfrage)
	    {
	        var produkte = AlleProduktInfos()
	            .Select(_ => new ProduktInfoEx
	            {
	                Id = _.Id,
	                Bezeichnung = _.Bezeichnung,
	                Verfuegbar = _lagerbestand.Verfuegbar_fuer(_.Id) - _auftraege.OffeneMenge_fuer(_.Id),
                    LagerBestand = _lagerbestand.LagerBestand_fuer(_.Id)
                })
	            .ToList();
            return new ProduktlisteEx { Produkte = produkte };
        }

        private Lagerbestandsliste Handle(QueryEnvelope queryEnvelope, LagerbestandsAbfrage abfrage)
        {
            return new Lagerbestandsliste { Bestand = _lagerbestand.Alle(abfrage.LagerId, ProduktProjektion.AlleIDs(_eventStore.History).ToList()) };
        }

        private Bestellungenliste Handle(QueryEnvelope queryEnvelope, OffeneBestellungenAbfrage abfrage)
		{
			var result = new Bestellungenliste { Bestellungen = AuftragProjektion.AlleIDs(_eventStore.History).Select(_auftraege.Access).Where(_=>!_.Erfuellt).ToList() };
			foreach (var bestellung in result.Bestellungen)
			{
				bestellung.Kundenname = _kunden.Access(bestellung.Kunde).Name;
				bestellung.Produktname = _produkte.Access(bestellung.Produkt).Bezeichnung;
			}
			return result;
		}




		private Protokoll Handle(QueryEnvelope queryEnvelope, ProtokollAbfrage abfrage)
		{
			return new Protokoll
				{
					Eintraege =
						_eventStore.History.Select(_ => new Eintrag {Info = _.Zeitpunkt.ToString("dd HH:mm:ss.fff ") + Alias(_.ToString())})
								   .Reverse().ToList()
				};
		}

		private string Alias(string input)
		{
			return ReplaceGuidsWith(input, _meta.Alias);
		}

		private string ReplaceGuidsWith(string input, Func<Guid, string> replacements)
		{
			const string guidPattern = @"(\{ID:([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\})";
			return Regex.Replace(input, guidPattern, match => replacements(MatchedGuid(match)));
		}

		private static Guid MatchedGuid(Match match)
		{
			return new Guid(match.Value.Substring(4,36));
		}


		private WarenkorbInfo Handle(QueryEnvelope queryEnvelope, WarenkorbAbfrage abfrage)
		{
		    var aktueller_warenkorb = _kunden.Access(abfrage.Kunde).Warenkorb;
		    var wk = _warenkoerbe.Access(aktueller_warenkorb);
		    return wk;
		}

	}




}
