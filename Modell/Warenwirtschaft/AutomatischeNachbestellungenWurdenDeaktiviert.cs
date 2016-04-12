using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class AutomatischeNachbestellungenWurdenDeaktiviert
    {
        public Guid Produkt;

        public override string ToString()
        {
            return "Für das Produkt {ID:" + Produkt + "} wurden automatische Nachbestellungen deaktiviert.";
        }
    }
}
