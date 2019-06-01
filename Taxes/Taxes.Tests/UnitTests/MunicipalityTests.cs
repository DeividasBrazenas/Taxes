using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Taxes.Service.Controllers;
using Taxes.Service.DataLayer;
using Taxes.Service.DataLayer.Models;
using Taxes.Tests.UnitTests.Helpers;

namespace Taxes.Tests.UnitTests
{
    [TestFixture]
    public class MunicipalityTests
    {
        private TaxesContext Context;
        private BaseController<Municipality> Controller;

        [SetUp]
        public void Init()
        {
            Context = DatabaseMock.GetDataBaseMock(TestData.Municipalities, TestData.Taxes).Object;
            Controller = new BaseController<Municipality>(Context);
        }

        [Test]
        public void GetMunicipality_Succeeds()
        {
            var municipalityObjectResult = Controller.Get(1) as OkObjectResult;
            var municipality = municipalityObjectResult.Value as Municipality;

            Assert.IsNotNull(municipality);
            Assert.AreEqual(1, municipality.Id);
            Assert.AreEqual("Vilnius", municipality.Name);
            Assert.AreEqual(4, municipality.Taxes.Count);
        }

        [Test]
        public void GetMunicipality_Fails()
        {
            var municipalityObjectResult = Controller.Get(999) as OkObjectResult;
            var municipality = municipalityObjectResult.Value as Municipality;

            Assert.IsNull(municipality);
        }

        [Test]
        public void GetMunicipalities_Succeeds()
        {
            var municipalityObjectResult = Controller.Get() as OkObjectResult;
            var municipalities = municipalityObjectResult.Value as IQueryable<Municipality>;

            Assert.IsNotNull(municipalities);
            Assert.AreEqual(2, municipalities.Count());
        }
    }
}