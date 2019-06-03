using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Taxes.Service;

namespace Taxes.Tests.IntegrationTests.Helpers
{
    public class TestClientProvider
    {
        public HttpClient Client { get; }

        public TestClientProvider()
        {
            var configuration = InitConfiguration();
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>().UseSetting("ConnectionStrings:DefaultConnection", configuration.GetConnectionString("DefaultConnection")));

            Client = server.CreateClient();
        }

        public static IConfiguration InitConfiguration()
        {
            var directory =  Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            return new ConfigurationBuilder().AddJsonFile(directory + "appsettings.tests.json").Build();
        }
    }
}
