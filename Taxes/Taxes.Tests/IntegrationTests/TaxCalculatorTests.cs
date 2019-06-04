using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Taxes.Service.DataLayer.Models;
using Taxes.Tests.IntegrationTests.Helpers;

namespace Taxes.Tests.IntegrationTests
    {
    [TestFixture]
    public class TaxCalculatorTests
        {
        private HttpClient _client;
        private JObject _municipality;

        [OneTimeSetUp]
        public async Task Init ()
            {
            _client = new TestClientProvider().Client;
            }

        [Test]
        [TestCase("2016-01-01", 0.1)]
        [TestCase("2016-05-02", 0.4)]
        [TestCase("2016-07-10", 0.2)]
        [TestCase("2016-03-16", 0.2)]
        [TestCase("2019-06-01", 0)]
        public async Task CalculateTax_ValidPayload_Succeeds (string dateString, double value)
            {
            string name = Guid.NewGuid().ToString("N");
            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);

            JObject tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01, 00, 00, 00),
                new DateTime(2016, 12, 31), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 05, 01),
                new DateTime(2016, 05, 31), TaxFrequency.Monthly, 0.4);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01),
                new DateTime(2016, 01, 01), TaxFrequency.Daily, 0.1);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 12, 25),
                new DateTime(2016, 12, 25), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            var response = await _client.GetAsync($"/odata/municipalitywithtax?name='{name}'&date={dateString}");
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(name, responseBody["value"][0]["Name"].ToObject<string>());
            Assert.AreEqual(municipality["value"][0]["Id"].ToObject<int>(), responseBody["value"][0]["Id"].ToObject<int>());
            Assert.AreEqual(value, responseBody["value"][0]["TaxValue"].ToObject<double>());
            }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task CalculateTax_InvalidName_Fails (object invalidName)
            {
            string name = Guid.NewGuid().ToString("N");
            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);

            JObject tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01, 00, 00, 00),
                new DateTime(2016, 12, 31), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 05, 01),
                new DateTime(2016, 05, 31), TaxFrequency.Monthly, 0.4);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01),
                new DateTime(2016, 01, 01), TaxFrequency.Daily, 0.1);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 12, 25),
                new DateTime(2016, 12, 25), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            var response = await _client.GetAsync($"/odata/municipalitywithtax?name='{invalidName}'&date=2019-06-01");
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Municipality name cannot be empty", responseBody["value"].ToObject<string>());
            }

        [Test]
        public async Task CalculateTax_NoDateProvided_Fails ()
            {
            string name = Guid.NewGuid().ToString("N");
            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);

            JObject tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01, 00, 00, 00),
                new DateTime(2016, 12, 31), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 05, 01),
                new DateTime(2016, 05, 31), TaxFrequency.Monthly, 0.4);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01),
                new DateTime(2016, 01, 01), TaxFrequency.Daily, 0.1);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 12, 25),
                new DateTime(2016, 12, 25), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            var response = await _client.GetAsync($"/odata/municipalitywithtax?name='{name}'");
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Date was not provided or it is not valid", responseBody["value"].ToObject<string>());
            }


        [Test]
        [TestCase("ItsMeMario")]
        [TestCase(true)]
        [TestCase(0.1)]
        public async Task CalculateTax_NonExistingName_Fails (object invalidName)
            {
            string name = Guid.NewGuid().ToString("N");
            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);

            JObject tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01, 00, 00, 00),
                new DateTime(2016, 12, 31), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 05, 01),
                new DateTime(2016, 05, 31), TaxFrequency.Monthly, 0.4);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 01, 01),
                new DateTime(2016, 01, 01), TaxFrequency.Daily, 0.1);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            tax = await HelperFunctions.AddTaxTest(_client, municipality["value"][0]["Id"].ToObject<int>(), new DateTime(2016, 12, 25),
                new DateTime(2016, 12, 25), TaxFrequency.Yearly, 0.2);
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            var response = await _client.GetAsync($"/odata/municipalitywithtax?name='{invalidName}'&date=2019-06-01");
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("No municipalities have been found with the provided name", responseBody["value"].ToObject<string>());
            }
        }
    }