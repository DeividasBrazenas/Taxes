using System;
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
        public async Task Init()
        {
            _client = new TestClientProvider().Client;
        }

        [Test]
        [TestCase("2016-01-01", 0.1)]
        [TestCase("2016-05-02", 0.4)]
        [TestCase("2016-07-10", 0.2)]
        [TestCase("2016-03-16", 0.2)]
        [TestCase("2019-06-01", 0)]
        public async Task CalculateTax_Succeeds(string dateString, double value)
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

            var response = await _client.GetAsync($"/odata/municipalitywithtax?name={name}&date={dateString}");
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

        }
    }
}