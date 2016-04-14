using System;
using System.Collections.Generic;
using System.Linq;

namespace Modell_shared
{
    public static class Lagerliste
    {
        public static readonly LagerInfo Hamburg = new LagerInfo(new Guid("234AA8B0-EB6E-48F4-A61A-20C5F292EEFE"), "Hamburg");
        public static readonly LagerInfo Muenchen = new LagerInfo(new Guid("7B287D85-C94C-491C-88FA-FAF31D6BE0C5"), "Muenchen");
        public static readonly List<LagerInfo> Alle = new[] {Hamburg, Muenchen}.ToList();
    }

}
