using database_analysis.Models;
using System.Data.Common;
using MongoDB.Driver;


namespace database_analysis.Interfaces
{
    public interface IDatabaseConnectionService
    {
        bool Connect(DatabaseConnectionDetails connectionDetails, out string errorMessage);
        DbConnection GetConnection();
        IMongoDatabase GetMongoDatabase();
    }
}