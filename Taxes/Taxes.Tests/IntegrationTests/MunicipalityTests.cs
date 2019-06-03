using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Taxes.Tests.IntegrationTests.Helpers;

namespace Taxes.Tests.IntegrationTests
{
    [TestFixture]
    public class MunicipalityTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void Init()
        {
            _client = new TestClientProvider().Client;
        }

        #region GET

        [Test]
        public async Task GetMunicipality_Succeeds()
        {
            JObject municipality = await HelperFunctions.AddMunicipalityTest(_client, Guid.NewGuid().ToString("N"));

            var response = await _client.GetAsync($"/odata/municipalities/{municipality["Id"]}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(1.1)]
        public async Task GetMunicipality_Fails(object id)
        {
            var response = await _client.GetAsync($"/odata/municipalities/{id}");
            Assert.IsTrue((HttpStatusCode.NoContent == response.StatusCode) || (HttpStatusCode.NotFound == response.StatusCode));
        }

        [Test]
        public async Task GetMunicipalities_Succeeds()
        {
            var response = await _client.GetAsync("/odata/municipalities");
            JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region POST

        [Test]
        public async Task AddMunicipality_Succeeds()
        {
            string name = Guid.NewGuid().ToString("N");

            await HelperFunctions.AddMunicipalityTest(_client, name);
            await HelperFunctions.GetMunicipalityTest(_client, name);
        }

        [Test]
        public async Task AddMunicipality_Fails()
        {
            dynamic payload = new JObject();
            payload.ItsMeMario = Guid.NewGuid().ToString("N");
            string expectedError = "The property 'ItsMeMario' does not exist on type 'Taxes.Service.DataLayer.Models.Municipality'. Make sure to only use property names that are defined by the type.";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/odata/municipalities", content);
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedError, responseBody["value"][0].ToObject<string>());
        }

        #endregion

        #region PATCH

        [Test]
        public async Task PatchMunicipality_Succeeds()
        {
            string name = Guid.NewGuid().ToString("N");
            string newName = Guid.NewGuid().ToString("N");

            dynamic payload = new JObject();
            payload.Name = newName;

            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/odata/municipalities/{municipality["value"][0]["Id"].ToObject<int>()}", content);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            await HelperFunctions.GetMunicipalityTest(_client, newName);
        }

        [Test]
        public async Task PatchMunicipality_FailsBadRequest()
        {
            string name = Guid.NewGuid().ToString("N");
            string expectedError = "The property 'ItsMeMario' does not exist on type 'Taxes.Service.DataLayer.Models.Municipality'. Make sure to only use property names that are defined by the type.";

            dynamic payload = new JObject();
            payload.ItsMeMario = Guid.NewGuid().ToString("N");

            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/odata/municipalities/{municipality["value"][0]["Id"].ToObject<int>()}", content);
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
        public async Task PatchMunicipality_FailsNotFound(object id)
        {
            dynamic payload = new JObject();
            payload.Name = Guid.NewGuid().ToString("N");

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/odata/municipalities/{id}", content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region PUT

        [Test]
        public async Task PutMunicipality_Succeeds()
        {
            string name = Guid.NewGuid().ToString("N");
            string newName = Guid.NewGuid().ToString("N");

            dynamic payload = new JObject();
            payload.Name = newName;

            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);
            payload.Id = municipality["value"][0]["Id"].ToObject<int>();

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/odata/municipalities/{municipality["value"][0]["Id"].ToObject<int>()}", content);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            await HelperFunctions.GetMunicipalityTest(_client, newName);
        }

        [Test]
        public async Task PutMunicipality_FailsBadRequest()
        {
            string name = Guid.NewGuid().ToString("N");
            string expectedError = "The property 'ItsMeMario' does not exist on type 'Taxes.Service.DataLayer.Models.Municipality'. Make sure to only use property names that are defined by the type.";

            dynamic payload = new JObject();
            payload.ItsMeMario = Guid.NewGuid().ToString("N");

            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);
            payload.Id = municipality["value"][0]["Id"].ToObject<int>();

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/odata/municipalities/{municipality["value"][0]["Id"].ToObject<int>()}", content);
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedError, responseBody["value"][0].ToObject<string>());
        }

        #endregion

        #region DELETE

        [Test]
        public async Task DeleteMunicipality_Succeeds()
        {
            string name = Guid.NewGuid().ToString("N");

            await HelperFunctions.AddMunicipalityTest(_client, name);
            JObject municipality = await HelperFunctions.GetMunicipalityTest(_client, name);

            var response = await _client.DeleteAsync($"/odata/municipalities/{municipality["value"][0]["Id"].ToObject<int>()}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            response = await _client.GetAsync($"/odata/municipalities/{municipality["value"][0]["Id"]}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(1.1)]
        public async Task DeleteMunicipality_Fails(object id)
        {
            var response = await _client.DeleteAsync($"/odata/municipalities/{id}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}
