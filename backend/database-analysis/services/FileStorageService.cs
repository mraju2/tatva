using database_analysis.Interfaces;

public class FileStorageService : IFileStorageService
{
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(ILogger<FileStorageService> logger)
    {
        _logger = logger;
    }

    public bool SaveToFile(string content, string fileName, out string errorMessage)
    {
        errorMessage = string.Empty;
        try
        {
            File.WriteAllText(fileName, content);
            _logger.LogInformation("File saved successfully at {0}", Path.GetFullPath(fileName));
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            _logger.LogError("File save failed: {0}", ex.Message);
            return false;
        }
    }
}