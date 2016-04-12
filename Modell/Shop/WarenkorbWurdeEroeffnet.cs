using System;

namespace Modell.Shop
{
    public sealed class WarenkorbWurdeEroeffnet
    {
        public Guid Warenkorb;
        public Guid Kunde;

        public override string ToString()
        {
            return "Warenkorb " + Warenkorb + " für Kunde {ID:" + Kunde + "}.";
        }
    }
}