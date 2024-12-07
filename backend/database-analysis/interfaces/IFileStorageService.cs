
namespace database_analysis.Interfaces
{

    public interface IFileStorageService
    {
        bool SaveToFile(string content, string fileName, out string errorMessage);
    }
}