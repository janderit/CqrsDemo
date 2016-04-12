using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests
{
    [TestFixture]
    public class Kundenverwaltung : Spezifikation
    {
        [Test]
        public void Ein_erfasster_Kunde_kann_abgerufen_werden()
        {
            var testsystem = Erzeuge_TestSystem();

            var kundenid = Neue_KundenId(testsystem);
            KundeErfassen(testsystem, kundenid, "TestName", "TestAnschrift");

            var kunde = KundeAbrufen(testsystem, kundenid);

            kunde.Should().NotBeNull();
            kunde.Id.Should().Be(kundenid);
            kunde.Name.Should().Be("TestName");
            kunde.Anschrift.Should().Be("TestAnschrift");
        }

        [Test]
        public void Ein_erfasster_Kunde_ist_in_der_Kundenliste_verzeichnet()
        {
            var testsystem = Erzeuge_TestSystem();

            var kundenid = Neue_KundenId(testsystem);
            KundeErfassen(testsystem, kundenid, "TestName", "TestAnschrift");

            var kundenliste = Kundenliste(testsystem, _ => _.Id == kundenid);
            kundenliste.Should().NotBeNull();
            kundenliste.Should().HaveCount(1);
            var kunde = kundenliste.Single();
            kunde.Should().NotBeNull();
            kunde.Id.Should().Be(kundenid);
            kunde.Name.Should().Be("TestName");
            kunde.Anschrift.Should().Be("TestAnschrift");
        }

        [Test]
        public void Die_Adressaenderung_eines_Kunden_ist_in_den_Kundeninformationen_sichtbar()
        {
            var testsystem = Erzeuge_TestSystem();
            var kundenid = Neue_KundenId(testsystem);
            KundeErfassen(testsystem, kundenid, "TestName", "Alte_Anschrift");

            AnschriftAendern(testsystem, kundenid, "Neue_Anschrift");

            var kunde = KundeAbrufen(testsystem, kundenid);
            kunde.Anschrift.Should().Be("Neue_Anschrift");
        }
    }
}
