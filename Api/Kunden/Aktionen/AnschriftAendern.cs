using System;
using Infrastruktur.Messaging;

namespace Api.Kunden.Aktionen
{
    public struct AnschriftAendern : Command
    {
        public AnschriftAendern(Guid kundenId, string neueAnschrift)
        {
            KundenId = kundenId;
            NeueAnschrift = neueAnschrift;
        }

        public readonly Guid KundenId;
        public readonly string NeueAnschrift;
    }
}
