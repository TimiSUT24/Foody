using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitFoody.Common
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly TestcontainersFixture _containers;

        public CustomWebApplicationFactory(TestcontainersFixture containers)
        {
            _containers = containers;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureAppConfiguration((ctx, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("appsettings.Test.json", optional: true);

                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Postgres"] = _containers.Postgres.GetConnectionString(),
                    ["Redis:ConnectionString"] = _containers.Redis.GetConnectionString(),
                    ["RabbitMQ:Host"] = _containers.RabbitMq.Hostname,
                    ["RabbitMQ:Port"] = _containers.RabbitMq.GetMappedPublicPort(5672).ToString(),
                    ["RabbitMQ:Username"] = "guest",
                    ["RabbitMQ:Password"] = "guest"
                });
            });
        }
    }
}
