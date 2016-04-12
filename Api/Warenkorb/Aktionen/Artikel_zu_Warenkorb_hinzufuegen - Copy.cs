using System;
using Infrastruktur.Messaging;

namespace Api.Warenkorb.Aktionen
{
    public struct ArtikelZuWarenkorbHinzufuegen : Command
    {
        public ArtikelZuWarenkorbHinzufuegen(Guid warenkorb, Guid produkt, int menge)
        {
            Warenkorb = warenkorb;
            Produkt = produkt;
            Menge = menge;
        }

        public readonly Guid Warenkorb;
        public readonly Guid Produkt;
        public readonly int Menge;
    }
}