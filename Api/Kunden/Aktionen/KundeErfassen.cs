using System;
using Infrastruktur.Messaging;

namespace Api.Kunden.Aktionen
{
    public struct KundeErfassen : Command
    {
        public KundeErfassen(Guid kundenId, string name, string anschrift)
        {
            KundenId = kundenId;
            Name = name;
            Anschrift = anschrift;
        }

        public Guid KundenId;
        public string Name;
        public string Anschrift;
    }
}
