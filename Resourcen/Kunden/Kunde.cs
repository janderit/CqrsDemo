using System;
using System.Collections.Generic;
using System.Text;

namespace Resourcen.Kunden
{
    public sealed class Kunde
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Anschrift { get; set; }
    }
}
