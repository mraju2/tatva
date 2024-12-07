
using database_analysis.Models;

namespace database_analysis.Interfaces
{
    public interface IDatabaseExtractionService
    {
        void RunExtraction(DatabaseConnectionDetails connectionDetails);
    }
}