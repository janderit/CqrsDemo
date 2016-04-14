using System;

namespace Resourcen.Warenwirtschaft
{
    public sealed class LagerbestandInfo
    {
        public Guid Lager { get; set; }
        public Guid Produkt { get; set; }
        public string Produktbezeichnung { get; set; }
        public int LagerBestand { get; set; }
        public bool Nachbestellt { get; set; }
        public int MengeImZulauf { get; set; }
        public bool AutomatischeNachbestellungen { get; set; }
    }
}