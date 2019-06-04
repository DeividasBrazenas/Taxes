using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Tests.IntegrationTests.Helpers
{
    public static class HelperFunctions
    {
        public static async Task<JObject> AddMunicipalityTest(HttpClient client, string name)
        {
            dynamic payload = new JObject();
            payload.Name = name;

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/odata/municipalities", content);
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(name, responseBody["Name"].ToObject<string>());

            return responseBody;
        }

        public static async Task<JObject> GetMunicipalityTest(HttpClient client, string name)
        {
            var response = await client.GetAsync($"/odata/municipalities?$filter=Name%20eq%20%27{name}%27");
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(name, responseBody["value"][0]["Name"].ToObject<string>());

            return responseBody;
        }

        public static async Task<JObject> AddTaxTest(HttpClient client, int municipalityId, DateTimeOffset startDate, DateTimeOffset endDate, TaxFrequency frequency = TaxFrequency.Yearly, double value = 0.5)
        {
            dynamic payload = new JObject();
            payload.MunicipalityId = municipalityId;
            payload.Frequency = Enum.GetName(frequency.GetType(), frequency);
            payload.StartDate = startDate;
            payload.EndDate = endDate;
            payload.Value = value;

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/odata/taxes", content);
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(municipalityId, responseBody["MunicipalityId"].ToObject<int>());
            Assert.AreEqual(frequency, responseBody["Frequency"].ToObject<TaxFrequency>());
            Assert.AreEqual(startDate, responseBody["StartDate"].ToObject<DateTimeOffset>());
            Assert.AreEqual(endDate, responseBody["EndDate"].ToObject<DateTimeOffset>());
            Assert.AreEqual(value, responseBody["Value"].ToObject<double>());

            return responseBody;
        }

        public static async Task<JObject> GetTaxTest(HttpClient client, int id)
        {
            var response = await client.GetAsync($"/odata/taxes/{id}");
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(id, responseBody["Id"].ToObject<int>());

            return responseBody;
        }
    }
}