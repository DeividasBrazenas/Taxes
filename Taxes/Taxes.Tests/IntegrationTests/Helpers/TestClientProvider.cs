using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Taxes.Service;

namespace Taxes.Tests.IntegrationTests.Helpers
    {
    public class TestClientProvider
        {
            public HttpClient Client { get; }

            public TestClientProvider()
            {
                var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

                Client = server.CreateClient();
            }
        }
    }
