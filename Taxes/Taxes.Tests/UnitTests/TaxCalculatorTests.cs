using System;
using NUnit.Framework;
using Taxes.Service.BusinessLogic;
using Taxes.Tests.UnitTests.Helpers;

namespace Taxes.Tests.UnitTests
{
    [TestFixture]
    class TaxCalculatorTests
    {
        [Test]
        [TestCase(2016, 01, 01, 0.1)]
        [TestCase(2016, 05, 02, 0.4)]
        [TestCase(2016, 07, 10, 0.2)]
        [TestCase(2016, 03, 16, 0.2)]
        [TestCase(2019, 06, 01, 0)]
        public void CalculateTax_ValidPayload_Succeeds(int year, int month, int day, double value)
        {
            var municipality = TestData.Municipalities[0];
            municipality.Taxes = TestData.Taxes;
            var date = new DateTime(year, month, day);

            var municipalityWithTax = TaxCalculator.CalculateTax(municipality, date);

            Assert.AreEqual(municipality.Name, municipalityWithTax.Name);
            Assert.AreEqual(value, municipalityWithTax.TaxValue);
        }

        [Test]
        public void CalculateTax_NoTaxes_WorksAsExpected()
        {
            var municipality = TestData.Municipalities[0];
            municipality.Taxes = TestData.Taxes;

            var municipalityWithTax = TaxCalculator.CalculateTax(municipality, DateTime.Now);

            Assert.AreEqual(municipality.Name, municipalityWithTax.Name);
            Assert.AreEqual(0, municipalityWithTax.TaxValue);
        }
    }
}
