using System;

namespace Resourcen.Warenwirtschaft
{
    public sealed class ProduktInfo
    {
        public Guid Id { get; set; }
        public string Bezeichnung { get; set; }
    }

    public sealed class ProduktInfoEx
    {
        public Guid Id { get; set; }
        public string Bezeichnung { get; set; }
        public int Verfuegbar { get; set; }
        public int LagerBestand { get; set; }
    }
}
