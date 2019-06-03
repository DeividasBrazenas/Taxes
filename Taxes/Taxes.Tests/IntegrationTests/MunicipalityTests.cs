using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Taxes.Service.DataLayer.Models;
using Taxes.Tests.IntegrationTests.Helpers;

namespace Taxes.Tests.IntegrationTests
    {
    [TestFixture]
    public class MunicipalityTests
    {
        private HttpClient Client;

            [SetUp]
            public void Init()
            {
                Client = new TestClientProvider().Client;
            }

            [Test]
            public async Task GetMunicipality_Succeeds()
            {
                var response = await Client.GetAsync("/odata/municipalities");
            }
        }
    }
