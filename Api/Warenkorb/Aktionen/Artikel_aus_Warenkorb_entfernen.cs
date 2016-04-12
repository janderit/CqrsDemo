using System;
using Infrastruktur.Messaging;

namespace Api.Warenkorb.Aktionen
{
    public struct ArtikelAusWarenkorbEntfernen : Command
    {
        public ArtikelAusWarenkorbEntfernen(Guid warenkorb, Guid zeile)
        {
            Warenkorb = warenkorb;
            Zeile = zeile;
        }

        public readonly Guid Warenkorb;
        public readonly Guid Zeile;
    }
}