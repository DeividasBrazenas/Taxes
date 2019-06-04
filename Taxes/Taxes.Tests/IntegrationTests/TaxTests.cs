using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Taxes.Service.DataLayer.Models;
using Taxes.Tests.IntegrationTests.Helpers;

namespace Taxes.Tests.IntegrationTests
{
    [TestFixture]
    public class TaxTests
    {
        private HttpClient _client;
        private JObject _municipality;

        [OneTimeSetUp]
        public async Task Init()
        {
            _client = new TestClientProvider().Client;

            dynamic payload = new JObject();
            payload.Name = Guid.NewGuid().ToString("N");

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/odata/municipalities", content);
            _municipality = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        #region GET

        [Test]
        public async Task GetTax_ValidPayload_Succeeds()
        {
            JObject tax = await HelperFunctions.AddTaxTest(_client, _municipality["Id"].ToObject<int>(), DateTime.Now, DateTime.Now.AddYears(1));

            var response = await _client.GetAsync($"/odata/taxes/{tax["Id"].ToObject<int>()}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(1.1)]
        public async Task GetTax_InvalidEntityId_Fails(object id)
        {
            var response = await _client.GetAsync($"/odata/taxes/{id}");
            Assert.IsTrue((HttpStatusCode.NoContent == response.StatusCode) || (HttpStatusCode.NotFound == response.StatusCode));
        }

        [Test]
        public async Task GetTaxes_NoPayload_Succeeds()
        {
            var response = await _client.GetAsync("/odata/taxes");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region POST

        [Test]
        public async Task AddTax_ValidPayload_Succeeds()
        {
            JObject tax = await HelperFunctions.AddTaxTest(_client, _municipality["Id"].ToObject<int>(), DateTime.Now, DateTime.Now.AddYears(1));
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());
        }

        [Test]
        public async Task AddTax_InvalidPayload_Fails()
        {
            dynamic payload = new JObject();
            payload.ItsMeMario = Guid.NewGuid().ToString("N");
            string expectedError = "The property 'ItsMeMario' does not exist on type 'Taxes.Service.DataLayer.Models.Tax'. Make sure to only use property names that are defined by the type.";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/odata/taxes", content);
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedError, responseBody["value"][0].ToObject<string>());
        }

        #endregion

        #region PATCH

        [Test]
        public async Task PatchTax_ValidPayload_Succeeds()
        {
            double newValue = 0.1;

            dynamic payload = new JObject();
            payload.Value = newValue;

            JObject tax = await HelperFunctions.AddTaxTest(_client, _municipality["Id"].ToObject<int>(), DateTime.Now, DateTime.Now.AddYears(1));
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/odata/taxes/{tax["Id"].ToObject<int>()}", content);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            tax = await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());
            Assert.AreEqual(newValue, tax["Value"].ToObject<double>());
        }

        [Test]
        public async Task PatchTax_InvalidPayload_Fails()
        {
            string expectedError = "The property 'ItsMeMario' does not exist on type 'Taxes.Service.DataLayer.Models.Tax'. Make sure to only use property names that are defined by the type.";

            dynamic payload = new JObject();
            payload.ItsMeMario = Guid.NewGuid().ToString("N");

            JObject tax = await HelperFunctions.AddTaxTest(_client, _municipality["Id"].ToObject<int>(), DateTime.Now, DateTime.Now.AddYears(1));
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/odata/taxes/{tax["Id"].ToObject<int>()}", content);
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedError, responseBody["value"][0].ToObject<string>());
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(1.1)]
        public async Task PatchTax_InvalidEntityId_Fails(object id)
        {
            dynamic payload = new JObject();
            payload.Value = 0.1;

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/odata/taxes/{id}", content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region PUT

        [Test]
        public async Task PutTax_ValidPayload_Succeeds()
        {
            TaxFrequency newFrequency = TaxFrequency.Daily;
            DateTime newDate = DateTime.Now.AddDays(1);
            double newValue = 0.15;

            dynamic payload = new JObject();
            payload.MunicipalityId = _municipality["Id"].ToObject<int>();
            payload.Frequency = Enum.GetName(newFrequency.GetType(), newFrequency); ;
            payload.StartDate = newDate;
            payload.EndDate = newDate;
            payload.Value = newValue;

            JObject tax = await HelperFunctions.AddTaxTest(_client, _municipality["Id"].ToObject<int>(), DateTime.Now, DateTime.Now.AddYears(1));
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());
            payload.Id = tax["Id"].ToObject<int>();

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/odata/taxes/{tax["Id"].ToObject<int>()}", content);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            tax = await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());
            Assert.AreEqual(newFrequency, tax["Frequency"].ToObject<TaxFrequency>());
            Assert.AreEqual(newDate, tax["StartDate"].ToObject<DateTime>());
            Assert.AreEqual(newDate, tax["EndDate"].ToObject<DateTime>());
            Assert.AreEqual(newValue, tax["Value"].ToObject<double>());
        }

        [Test]
        public async Task PutTax_InvalidPayload_Fails()
        {
            string expectedError = "The property 'ItsMeMario' does not exist on type 'Taxes.Service.DataLayer.Models.Tax'. Make sure to only use property names that are defined by the type.";

            dynamic payload = new JObject();
            payload.ItsMeMario = Guid.NewGuid().ToString("N");

            JObject tax = await HelperFunctions.AddTaxTest(_client, _municipality["Id"].ToObject<int>(), DateTime.Now, DateTime.Now.AddYears(1));
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());
            payload.Id = tax["Id"].ToObject<int>();

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/odata/taxes/{tax["Id"].ToObject<int>()}", content);
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedError, responseBody["value"][0].ToObject<string>());
        }

        #endregion

        #region DELETE

        [Test]
        public async Task DeleteTax_ValidEntityId_Succeeds()
        {
            JObject tax = await HelperFunctions.AddTaxTest(_client, _municipality["Id"].ToObject<int>(), DateTime.Now, DateTime.Now.AddYears(1));
            await HelperFunctions.GetTaxTest(_client, tax["Id"].ToObject<int>());

            var response = await _client.DeleteAsync($"/odata/taxes/{tax["Id"].ToObject<int>()}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            response = await _client.GetAsync($"/odata/taxes/{tax["Id"]}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(1.1)]
        public async Task DeleteTax_InvalidEntityId_Fails(object id)
        {
            var response = await _client.DeleteAsync($"/odata/taxes/{id}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}
