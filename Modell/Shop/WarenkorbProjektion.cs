using System;
using System.Collections.Generic;
using System.Linq;
using Infrastruktur.Common;
using Resourcen.Shop;

namespace Modell.Shop
{
    public sealed class WarenkorbProjektion
    {

        private readonly Func<IEnumerable<Ereignis>> _history;

        public WarenkorbProjektion(Guid id, Func<IEnumerable<Ereignis>> history)
        {
            _history = history;
            Id = id;
        }

        public readonly Guid Id;

        public Guid Kunde
        {
            get
            {
                return _history().OfType<Ereignis<WarenkorbWurdeEroeffnet>>().Single().Daten.Kunde;
            }
        }

        public IEnumerable<ArtikelImWarenkorb> Artikel
        {
            get
            {
                var result = new List<ArtikelImWarenkorb>();
                var h = _history();
                foreach (var e in h.OfType<Ereignis<ArtikelWurdeZuWarenkorbHinzugefuegt>>())
                {
                    result.Add(new ArtikelImWarenkorb
                        {
                            ZeileId=e.Daten.Zeile,
                            Produkt = e.Daten.Produkt,
                            Menge = e.Daten.Menge,
                            Bezeichnung = e.Daten.Produkt.ToString()
                        });
                }

                foreach (var e in h.OfType<Ereignis<ArtikelWurdeAusWarenkorbEntfernt>>())
                {
                    result.RemoveAll(_ => _.ZeileId == e.Daten.Zeile);
                }

                int i = 1;
                foreach (var artikelImWarenkorb in result)
                {
                    artikelImWarenkorb.Zeile = i++;
                }
                return result;
            }
        }

    }
}
