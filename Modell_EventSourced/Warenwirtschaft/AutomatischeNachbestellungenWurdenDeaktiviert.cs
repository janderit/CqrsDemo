using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Warenwirtschaft
{
    public sealed class AutomatischeNachbestellungenWurdenDeaktiviert
    {
        public Guid Lager;
        public Guid Produkt;

        public override string ToString()
        {
            return "Für das Produkt {ID:" + Produkt + "} wurden automatische Nachbestellungen an {ID:" + Lager + "} deaktiviert.";
        }
    }
}
