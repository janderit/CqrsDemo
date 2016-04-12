using System;
using Resourcen.Kunden;

namespace Api.Kunden
{
    public interface KundenApi
    {
        Kundenliste Kundenliste();
        KundeInfo Kunde(Guid id);
        void KundeErfassen(Guid id, string name, string anschrift);
        void AnschriftAendern(Guid id, string neueanschrift);
    }
}
