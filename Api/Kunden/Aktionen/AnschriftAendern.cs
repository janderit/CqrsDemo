using System;

namespace Api.Kunden.Aktionen
{
    public sealed class AnschriftAendern
    {
        public Guid KundenId { get; set; }
        public string NeueAnschrift { get; set; }
    }
}
