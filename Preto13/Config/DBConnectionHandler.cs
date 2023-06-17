using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Preto13.Config
{
    public class DatabaseConnection
    {
        private string _connectionString;

        public DatabaseConnection()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            _connectionString = configuration.GetConnectionString("databaseUrl");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
