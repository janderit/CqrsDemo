using System;

namespace Modell_shared
{
    public class LagerInfo
    {
        public LagerInfo(Guid id, string bezeichnung)
        {
            Id = id;
            Bezeichnung = bezeichnung;
        }

        public readonly Guid Id;
        public readonly string Bezeichnung;
    }
}