using System;

namespace Api.Kunden.Aktionen
{
    public sealed class KundeErfassen
    {
        public Guid KundenId { get; set; }
        public string Name { get; set; }
        public string Anschrift { get; set; }
    }
}
