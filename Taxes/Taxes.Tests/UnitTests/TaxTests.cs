using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Taxes.Service.Controllers;
using Taxes.Service.DataLayer;
using Taxes.Service.DataLayer.Models;
using Taxes.Tests.UnitTests.Helpers;

namespace Taxes.Tests.UnitTests
{
    [TestFixture]
    public class TaxTests
    {
        private TaxesContext Context;
        private BaseController<Tax> Controller;

        [SetUp]
        public void Init()
        {
            Context = DatabaseMock.GetDataBaseMock(TestData.Municipalities, TestData.Taxes).Object;
            Controller = new BaseController<Tax>(Context);
        }

        [Test]
        public void GetTax_Succeeds()
        {
            var taxObjectResult = Controller.Get(1) as OkObjectResult;
            var tax = taxObjectResult.Value as Tax;
            var expectedTax = TestData.Taxes.FirstOrDefault(x => x.Id == 1);

            Assert.IsNotNull(tax);
            Assert.AreEqual(expectedTax.Id, tax.Id);
            Assert.AreEqual(expectedTax.MunicipalityId, tax.MunicipalityId);
            Assert.AreEqual(expectedTax.StartDate, tax.StartDate);
            Assert.AreEqual(expectedTax.EndDate, tax.EndDate);
            Assert.AreEqual(expectedTax.Frequency, tax.Frequency);
            Assert.AreEqual(expectedTax.Value, tax.Value);
        }

        [Test]
        public void GetTask_Fails()
        {
            var taxObjectResult = Controller.Get(999) as OkObjectResult;
            var tax = taxObjectResult.Value as Tax;

            Assert.IsNull(tax);
        }

        [Test]
        public void GetTaxes_Succeeds()
        {
            var taxObjectResult = Controller.Get() as OkObjectResult;
            var tax = taxObjectResult.Value as IQueryable<Tax>;

            Assert.IsNotNull(tax);
            Assert.AreEqual(4, tax.Count());
        }
    }
}