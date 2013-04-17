using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class MindestVerfuegbarkeitWurdeDefiniert
    {
        public int Menge { get; set; }
        public override string ToString()
        {
            return "Für das Produkt wurde die Mindestverfügbarkeit für automatische Nachbestellungen definiert: "+Menge;
        }
    }
}
