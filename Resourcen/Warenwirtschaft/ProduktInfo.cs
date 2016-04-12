using System;
using System.Collections.Generic;
using System.Text;

namespace Resourcen.Warenwirtschaft
{
    public sealed class ProduktInfo
    {
        public Guid Id { get; set; }
        public string Bezeichnung { get; set; }
        public int LagerBestand { get; set; }
        public int Verfuegbar { get; set; }
        public bool Nachbestellt { get; set; }
        public int MengeImZulauf { get; set; }
        public bool AutomatischeNachbestellungen { get; set; }
    }
}
