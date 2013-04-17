using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Warenwirtschaft.Aktionen
{
    public sealed class MindestVerfuegbarkeitDefinieren
    {
        public Guid ProduktId { get; set; }
        public int MindestVerfuegbarkeit { get; set; }
        public int MindestBestellmenge { get; set; }
    }
}
