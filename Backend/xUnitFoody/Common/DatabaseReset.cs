using Npgsql;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitFoody.Common
{
    public class DatabaseReset
    {
        private readonly string _connectionString;
        private Respawner _respawner;

        public DatabaseReset(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InitializeAsync()
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" }
            });
        }

        public async Task ResetAsync()//use in tests
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await _respawner.ResetAsync(conn);
        }
    }
}
