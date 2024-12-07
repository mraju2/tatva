using database_analysis.Interfaces;
using database_analysis.Models;
using System.Data.Common;
using MongoDB.Driver;
using Microsoft.Data.SqlClient; // For SqlConnection
using Npgsql; // For NpgsqlConnection
using Oracle.ManagedDataAccess.Client; // For OracleConnection
using Microsoft.Data.Sqlite; // For SqliteConnection

namespace database_analysis.services
{

    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private DbConnection _connection;
        private IMongoDatabase _mongoDatabase;
        private readonly ILogger<DatabaseConnectionService> _logger;

        public DatabaseConnectionService(ILogger<DatabaseConnectionService> logger)
        {
            _logger = logger;
        }

        public bool Connect(DatabaseConnectionDetails connectionDetails, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var connectionString = connectionDetails.GetConnectionString();
                switch (connectionDetails.ProviderName)
                {
                    case "System.Data.SqlClient":
                        _connection = new SqlConnection(connectionString);
                        break;
                    case "Npgsql":
                        _connection = new NpgsqlConnection(connectionString);
                        break;
                    case "Oracle.ManagedDataAccess.Client":
                        _connection = new OracleConnection(connectionString);
                        break;
                    case "Microsoft.Data.Sqlite":
                        _connection = new SqliteConnection(connectionString);
                        break;
                    case "MySql.Data.MySqlClient":
                        _connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                        break;
                    case "MongoDB.Driver":
                        var mongoClient = new MongoClient(connectionString);
                        _mongoDatabase = mongoClient.GetDatabase(connectionDetails.DatabaseName);
                        _logger.LogInformation("MongoDB connected successfully.");
                        return true;
                    default:
                        throw new NotSupportedException("The specified provider is not supported.");
                }

                _connection.Open();
                _logger.LogInformation("Database connected successfully.");
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                _logger.LogError("Database connection failed: {0}", ex.Message);
                return false;
            }
        }

        public DbConnection GetConnection()
        {
            return _connection;
        }

        public IMongoDatabase GetMongoDatabase()
        {
            return _mongoDatabase;
        }
    }
}