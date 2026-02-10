using Application.Order.Handlers;
using Infrastructure.Data;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitFoody.Common
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private TestcontainersFixture? _containers;
        public TestcontainersFixture Containers => _containers ?? throw new InvalidOperationException("Containers not initialized");
        public IServiceProvider ServiceProvider => this.Services;
        public async Task InitializeAsync()
        {
            _containers = new TestcontainersFixture();
            await _containers.InitializeAsync();
        }

        public new async Task DisposeAsync()
        {
            if (_containers != null)
                await _containers.DisposeAsync();

            await base.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Issuer"] = "Foody",
                    ["Jwt:Audience"] = "Us",
                    ["Jwt:Key"] = "very-secret-key-test-key-3554555555534dsd-EXTRA-LONG",
                    ["Jwt:ExpireInMintues"] = "30",
                    ["Jwt:RefreshDays"] = "1",
                    ["CacheSettings:LongLivedMinutes"] = "2",
                    ["CacheSettings:ShortLivedMinutes"] = "1",
                });
            });

            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<FoodyDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Add DbContext with test container connection string
                services.AddDbContext<FoodyDbContext>(options =>
                {
                    options.UseNpgsql(_containers!.Postgres.GetConnectionString());
                });

                var distributedCacheDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDistributedCache));

                if (distributedCacheDescriptor != null)
                    services.Remove(distributedCacheDescriptor);

                // Remove existing IConnectionMultiplexer
                var redisDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IConnectionMultiplexer));

                if (redisDescriptor != null)
                    services.Remove(redisDescriptor);

                // Add Redis with container connection string
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = _containers!.Redis.GetConnectionString();
                });

                services.AddSingleton<IConnectionMultiplexer>(sp =>
                {
                    var configuration = _containers!.Redis.GetConnectionString();
                    var options = ConfigurationOptions.Parse(configuration);
                    options.AbortOnConnectFail = true;
                    return ConnectionMultiplexer.Connect(options);
                });

                var massTransitDescriptors = services
                .Where(d => d.ServiceType.Namespace?.StartsWith("MassTransit") == true)
                .ToList();

                foreach(var descriptor2 in massTransitDescriptors)
                {
                    services.Remove(descriptor2);
                }

                services.AddMassTransitTestHarness(x =>
                {
                    x.AddConsumer<BookShipmentConsumer>();
                    x.AddConsumer<UpdateOrderStatusConsumer>();
                    x.AddConsumer<SendOrderEmailConsumer>();

                    x.UsingInMemory((ctx, config) =>
                    {                       
                        config.ConfigureEndpoints(ctx);
                    });
                });
            });
        }
    }
}
