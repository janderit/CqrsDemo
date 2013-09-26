using System;

namespace Modell.Warenwirtschaft
{
    public sealed class ProduktWurdeEingelistet
    {
        public Guid Produkt;
        public string Bezeichnung;

        public override string ToString()
        {
            return "Produkt wurde eingelistet";
        }
    }
}
