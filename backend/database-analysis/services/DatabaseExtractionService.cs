using database_analysis.Interfaces;
using database_analysis.Models;
using MongoDB.Driver;
using System.Data.Common;

public class DatabaseExtractionService : IDatabaseExtractionService
{
    private readonly IDatabaseConnectionService _dbConnectionService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<DatabaseExtractionService> _logger;

    public DatabaseExtractionService(IDatabaseConnectionService dbConnectionService, IFileStorageService fileStorageService, ILogger<DatabaseExtractionService> logger)
    {
        _dbConnectionService = dbConnectionService;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public void RunExtraction(DatabaseConnectionDetails connectionDetails)
    {
        string errorMessage;

        // Step 1: Connect to the database
        if (!_dbConnectionService.Connect(connectionDetails, out errorMessage))
        {
            _logger.LogError("Extraction stopped due to connection failure.");
            throw new InvalidOperationException("Database connection failed: " + errorMessage);
        }

        // Step 2: Extract DDL Statements
        try
        {
            string ddlStatements;

            if (connectionDetails.ProviderName == "MongoDB.Driver")
            {
                var mongoDatabase = _dbConnectionService.GetMongoDatabase();
                ddlStatements = ExtractMongoDBStructure(mongoDatabase);
            }
            else
            {
                var connection = _dbConnectionService.GetConnection();
                ddlStatements = ExtractDatabaseStructure(connection);
            }

            // Step 3: Save DDL Statements Locally
            if (!_fileStorageService.SaveToFile(ddlStatements, "DatabaseStructure.sql", out errorMessage))
            {
                _logger.LogError("Extraction stopped due to file save failure.");
                throw new InvalidOperationException("File save failed: " + errorMessage);
            }

            _logger.LogInformation("Extraction completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during extraction: {0}", ex.Message);
            throw;
        }
    }

    private string ExtractDatabaseStructure(DbConnection connection)
    {
        _logger.LogInformation("Starting to extract database structure...");

        var ddlStatements = "";
        try
        {
            using (var command = connection.CreateCommand())
            {
                _logger.LogInformation("Preparing query to fetch table and column information...");

                // Query to retrieve table schema information for all tables
                command.CommandText = @"
                        SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_SCHEMA = 'sakila'
                        ORDER BY TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION;";

                _logger.LogInformation("Executing query to fetch table structure...");

                using (var reader = command.ExecuteReader())
                {
                    _logger.LogInformation("Query executed successfully. Processing results...");

                    string currentTable = null;
                    string currentSchema = null;
                    var tableDefinition = "";

                    while (reader.Read())
                    {
                        var schemaName = reader["TABLE_SCHEMA"].ToString();
                        var tableName = reader["TABLE_NAME"].ToString();
                        var columnName = reader["COLUMN_NAME"].ToString();
                        var dataType = reader["DATA_TYPE"].ToString();
                        var maxLength = reader["CHARACTER_MAXIMUM_LENGTH"]?.ToString() ?? "";

                        _logger.LogInformation("Processing column: {Schema}.{Table}.{Column} ({DataType})",
                            schemaName, tableName, columnName, dataType);

                        if (currentTable != tableName || currentSchema != schemaName)
                        {
                            // If switching to a new table, finalize the previous table definition
                            if (!string.IsNullOrEmpty(tableDefinition))
                            {
                                tableDefinition = tableDefinition.TrimEnd(',', '\n') + "\n);\n";
                                ddlStatements += tableDefinition;
                                _logger.LogInformation("Finished processing table: {Schema}.{Table}", currentSchema, currentTable);
                            }

                            // Start a new table definition
                            currentTable = tableName;
                            currentSchema = schemaName;
                            tableDefinition = $"CREATE TABLE {schemaName}.{tableName} (\n";
                            _logger.LogInformation("Started processing table: {Schema}.{Table}", schemaName, tableName);
                        }

                        // Append column definition
                        tableDefinition += $"  {columnName} {dataType}{(string.IsNullOrEmpty(maxLength) ? "" : $"({maxLength})")},\n";
                    }

                    // Finalize the last table definition
                    if (!string.IsNullOrEmpty(tableDefinition))
                    {
                        tableDefinition = tableDefinition.TrimEnd(',', '\n') + "\n);\n";
                        ddlStatements += tableDefinition;
                        _logger.LogInformation("Finished processing table: {Schema}.{Table}", currentSchema, currentTable);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error extracting database structure: {0}", ex.Message);
            throw;
        }

        _logger.LogInformation("Completed extracting database structure.");
        return ddlStatements;
    }


    private string ExtractMongoDBStructure(IMongoDatabase database)
    {
        // A simplified version of MongoDB structure extraction logic
        _logger.LogInformation("Extracting MongoDB structure...");
        // NOTE: In a real scenario, we would retrieve the collections and documents and construct a similar representation.
        return "-- MongoDB Collections and Documents (Simulated)";
    }
}