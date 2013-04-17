namespace Modell.Warenwirtschaft
{
    public sealed class ProduktWurdeEingelistet
    {
        public string Bezeichnung { get; set; }

        public override string ToString()
        {
            return "Produkt wurde eingelistet";
        }
    }
}
