using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using EFCore_Inheritance_Demo_Main9.Data;
using EFCore_Inheritance_Demo_Main9.Hubs;
using EFCore_Inheritance_Demo_Main9.Models;

namespace EFCore_Inheritance_Demo_Main9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInfoController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<LogInfoController> _logger;
        private readonly IHubContext<TodoHub> _hubContext;

        public LogInfoController(DataContext context, ILogger<LogInfoController> logger, IHubContext<TodoHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
        }

        // GET: api/GetLogInfoItems?userName=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogInfo>>> GetLogInfoItems(string userName = "Default User")
        {
            _logger.LogInformation($"[{userName}] Hentede alle LogInfo-items.");

            return await _context.LogInfoes.ToListAsync();
        }

        // GET: api/GetLogInfoItem? id=/userName=.
        [HttpGet("{id}")]
        public async Task<ActionResult<LogInfo>> GetLogInfoItem(long id, string userName)
        {
            // Logning af operationen
            _logger.LogInformation($"[{userName}] Hentede et enkelt todo-item med ID: {id}.");

            // Finder todo-elementet i databasen baseret på det medsendte ID
            var LogInfoItem = await _context.LogInfoes.FindAsync(id);

            // Tjekker om elementet blev fundet
            if (LogInfoItem == null)
            {
                _logger.LogWarning($"[{userName}] LogInfo-item med ID: {id} blev ikke fundet.");
                return NotFound(); // Returnerer en 404 Not Found-fejl
            }

            // Returnerer det fundne element
            return LogInfoItem;
        }
    }
}
