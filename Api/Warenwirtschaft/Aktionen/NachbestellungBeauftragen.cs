using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Warenwirtschaft.Aktionen
{
    public sealed class NachbestellungBeauftragen
    {
        public Guid ProduktId { get; set; }
        public int BestellteMenge { get; set; }
        public decimal VereinbarterEinkaufsPreisInEuro { get; set; }
        public DateTime AvisiertesLieferdatum  { get; set; }
    }
}
