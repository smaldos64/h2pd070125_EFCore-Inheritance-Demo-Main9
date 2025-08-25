using EFCore_Inheritance_Demo_Main9.Data;
using EFCore_Inheritance_Demo_Main9.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using EFCore_Inheritance_Demo_Main9.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using EFCore_Inheritance_Demo_Main9.Models;

namespace EFCore_Inheritance_Demo_Main9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseInfoController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<DatabaseInfoController> _logger;
        private readonly IHubContext<TodoHub> _hubContext;

        public DatabaseInfoController(DataContext context, ILogger<DatabaseInfoController> logger, IHubContext<TodoHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task <IActionResult> GetDatabaseInfo(string userName = "Default User")
        {
            // Hent forbindelsesstrengen fra DbContext
            var connectionString = _context.Database.GetDbConnection().ConnectionString;

            // Brug SqlConnectionStringBuilder til at parse strengen
            var builder = new SqlConnectionStringBuilder(connectionString);

            _logger.LogInformation($"[{userName}] Hentede Database Informationer.");

            LogInfo LogInfoObject = new LogInfo();
            LogInfoObject.LogInfoUserName = userName;
            LogInfoObject.LogInfoDescription = $"[{userName}] Hentede Database Informationer.";
            _context.LogInfoes.Add(LogInfoObject);
            await _context.SaveChangesAsync();

            // Returner informationen
            return Ok(new
            {
                server = builder.DataSource,
                database = builder.InitialCatalog
            });
        }
    }
}
