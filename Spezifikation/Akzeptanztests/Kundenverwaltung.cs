using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Spezifikation.Akzeptanztests
{
    [TestFixture]
    public class Kundenverwaltung : Akzeptanztest
    {
        [Test]
        public void Ein_erfasster_Kunde_ist_in_der_Kundenliste_verzeichnet()
        {
            var api = TestInstanz();
            var id = api.Kunden.KundeErfassen("TestName", "TestAnschrift");
            var kunden = api.Kunden.Kundenliste().Kunden;

            kunden.Should().NotBeNull();
            kunden.Should().HaveCount(1);
            
            var kunde = kunden.Single();
            kunde.Should().NotBeNull();
            kunde.Id.Should().Be(id);
            kunde.Name.Should().Be("TestName");
            kunde.Anschrift.Should().Be("TestAnschrift");
            
            kunde = api.Kunden.Kunde(id);
            kunde.Should().NotBeNull();
            kunde.Id.Should().Be(id);
            kunde.Name.Should().Be("TestName");
            kunde.Anschrift.Should().Be("TestAnschrift");
        }

        [Test]
        public void Die_Adressaenderung_eines_Kunden_ist_in_der_Kundenliste_sichtbar()
        {
            var api = TestInstanz();
            var id = api.Kunden.KundeErfassen("TestName", "AlteAnschrift");
            api.Kunden.AnschriftAendern(id, "NeueAnschrift");
            api.Kunden.Kunde(id).Anschrift.Should().Be("NeueAnschrift");
        }

    }
}
