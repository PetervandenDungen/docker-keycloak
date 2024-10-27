using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("CreateTable")]
        public async Task<IActionResult> CreateTable([FromBody] string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return BadRequest("Table name cannot be null or empty.");
            }

            // Ensure table name is valid and prevent SQL Injection by using parameterized SQL.
            var sanitizedTableName = new SqlParameter("tableName", tableName);
            var createTableSql = $"CREATE TABLE [{tableName}] (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100))";

            try
            {
                await _context.Database.ExecuteSqlRawAsync(createTableSql);
                return Ok($"Table '{tableName}' created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating table.");
                return StatusCode(500, "An error occurred while creating the table.");
            }
        }
    }
}

