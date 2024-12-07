using Microsoft.AspNetCore.Mvc;
using database_analysis.Interfaces;
using database_analysis.Models;

namespace database_analysis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseExtractionService _databaseExtractionService;
    private readonly ILogger<DatabaseController> _logger;

    public DatabaseController(IDatabaseExtractionService databaseExtractionService, ILogger<DatabaseController> logger)
    {
        _databaseExtractionService = databaseExtractionService;
        _logger = logger;
    }

    [HttpPost("extract")]
    public IActionResult ExtractDatabase([FromBody] DatabaseConnectionDetails connectionDetails)
    {
        try
        {
            _databaseExtractionService.RunExtraction(connectionDetails);
            return Ok("Database extraction completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database extraction.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during database extraction.");
        }
    }
}