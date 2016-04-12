using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class MindestVerfuegbarkeitWurdeDefiniert
    {
        public Guid Produkt;
        public int Menge;

        public override string ToString()
        {
            return "Für das Produkt {ID:" + Produkt + "} wurde die Mindestverfügbarkeit für automatische Nachbestellungen definiert: " + Menge + ".";
        }
    }
}
