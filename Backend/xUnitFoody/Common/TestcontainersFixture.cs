using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace xUnitFoody.Common
{
    public class TestcontainersFixture : IAsyncLifetime
    {
        public PostgreSqlContainer Postgres { get; private set; } = null!;
        public RedisContainer Redis { get; private set; } = null!;
        public RabbitMqContainer RabbitMQ { get; private set; } = null!;
        public DatabaseReset DbReset { get; private set; }

        public async Task InitializeAsync()
        {
            Postgres = new PostgreSqlBuilder("postgres:18")
                .WithDatabase("testdb")
                .WithUsername("test")
                .WithPassword("test")
                .Build();

            Redis = new RedisBuilder("redis:latest").Build();

            RabbitMQ = new RabbitMqBuilder("rabbitmq:4-management")
                .WithUsername("guest")
                .WithPassword("guest")
                .Build();


            await Postgres.StartAsync();
            await Redis.StartAsync();
            await RabbitMQ.StartAsync();

            var options = new DbContextOptionsBuilder<FoodyDbContext>()
            .UseNpgsql(Postgres.GetConnectionString())
            .Options;

            using (var db = new FoodyDbContext(options))
            {
                await db.Database.MigrateAsync(); // ensures tables exist
            }

            DbReset = new DatabaseReset(Postgres.GetConnectionString());
            await DbReset.InitializeAsync();
            
        }

        public async Task DisposeAsync()
        {
            await Postgres.DisposeAsync();
            await Redis.DisposeAsync();
            await RabbitMQ.DisposeAsync();
        }
    }
}
