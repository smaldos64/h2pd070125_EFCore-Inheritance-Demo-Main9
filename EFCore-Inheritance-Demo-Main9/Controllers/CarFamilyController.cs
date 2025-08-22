using EFCore_Inheritance_Demo_Main9.Data;
using EFCore_Inheritance_Demo_Main9.Hubs;
using EFCore_Inheritance_Demo_Main9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFCore_Inheritance_Demo_Main9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarFamilyController : ControllerBase
    {
        private DataContext _context;
        private readonly ILogger<CarFamilyController> _logger;
        private readonly IHubContext<TodoHub> _hubContext;

        public CarFamilyController(DataContext context, ILogger<CarFamilyController> logger, IHubContext<TodoHub> hubContext)
        {
            this._context = context;
            this._logger = logger;
            this._hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars(string userName = "Default User")
        {
            _logger.LogInformation($"[{userName}] Hentede alle Cars.");

            LogInfo LogInfoObject = new LogInfo();
            LogInfoObject.LogInfoUserName = userName;
            LogInfoObject.LogInfoDescription = $"Alle Cars læst af {userName}";
            _context.LogInfoes.Add(LogInfoObject);
            await _context.SaveChangesAsync();

            var cars = await _context.TPTCars.ToListAsync();
            return Ok(cars);
        }

        [HttpGet("GetCarById/{id}")]

        public async Task<IActionResult> GetCarById(long id, string userName)
        {
            var car = await _context.TPTCars.FindAsync(id);
            //var car = await _context.TPTCars
            //.OfType<TPTCarModel>() // Inkluderer CarModel-typen
            //.SingleOrDefaultAsync(c => c.Id == id);

            if (car == null) 
            {
                return NotFound();
            }

            // Logning af operationen
            _logger.LogInformation($"[{userName}] Hentede en enkelt Car med ID: {id}.");

            LogInfo LogInfoObject = new LogInfo();
            LogInfoObject.LogInfoUserName = userName;
            LogInfoObject.LogInfoDescription = $"Car med Id: {id} læst af {userName}";
            _context.LogInfoes.Add(LogInfoObject);
            await _context.SaveChangesAsync();

            // Tving serializeren til at anvende regler for polymorfi
            // ved at lave om til en liste.
            var cars = new List<TPTCar> { car };
            
            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCar([FromBody]TPTCar car, string userName = "Default User")
        {
            string logStringAdder;

            if (car == null)
            {
                return BadRequest();
            }

            //if (car is TPTCarModel carModel)
            //{
            //    // Denne blok vil kun blive eksekveret, hvis JSON-dataene
            //    // blev deserialiseret til et CarModel-objekt
            //    _context.TPTCars.Add(carModel);
            //}
            //else
            //{
            //    _context.TPTCars.Add(car);
            //}

            if (car is TPTCarModel carModel)
            {
                // Denne blok vil kun blive eksekveret, hvis JSON-dataene
                // blev deserialiseret til et CarModel-objekt
                logStringAdder = "CarModel";
            }
            else
            {
                logStringAdder = "Car";
            }
          
            _context.TPTCars.Add(car);
            await _context.SaveChangesAsync();

            LogInfo LogInfoObject = new LogInfo();
            LogInfoObject.LogInfoUserName = userName;
            LogInfoObject.LogInfoDescription = $"{logStringAdder} med Id: {car.carId} oprettet af {userName}";
            _context.LogInfoes.Add(LogInfoObject);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"[{userName}] Tilføjede {logStringAdder} med ID: {car.carId}.");
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", userName, "added");

            return Ok(car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] TPTCar car, string userName = "Default User")
        {
            string logStringAdder;

            if (id != car.carId)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TPTCars.Any(e => e.carId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (car is TPTCarModel carModel)
            {
                // Denne blok vil kun blive eksekveret, hvis JSON-dataene
                // blev deserialiseret til et CarModel-objekt
                logStringAdder = "CarModel";
            }
            else
            {
                logStringAdder = "Car";
            }

            LogInfo LogInfoObject = new LogInfo();
            LogInfoObject.LogInfoUserName = userName;
            LogInfoObject.LogInfoDescription = $"{logStringAdder} med Id: {car.carId} ændret af {userName}";
            _context.LogInfoes.Add(LogInfoObject);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"[{userName}] Opdaterede {logStringAdder} med ID: {car.carId}.");
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", userName, "updated");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(long id, string userName = "Default User")
        {
            string logStringAdder;

            // Find objektet ved hjælp af basisklassen
            var car = await _context.TPTCars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            // Fjern objektet. EF Core ved, hvilken type det er, og
            // håndterer sletningen af de relevante tabeller.
            _context.TPTCars.Remove(car);
            await _context.SaveChangesAsync();

            if (car is TPTCarModel carModel)
            {
                // Denne blok vil kun blive eksekveret, hvis JSON-dataene
                // blev deserialiseret til et CarModel-objekt
                logStringAdder = "CarModel";
            }
            else
            {
                logStringAdder = "Car";
            }

            LogInfo LogInfoObject = new LogInfo();
            LogInfoObject.LogInfoUserName = userName;
            LogInfoObject.LogInfoDescription = $"{logStringAdder} med Id: {id} slettet af {userName}";
            _context.LogInfoes.Add(LogInfoObject);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"[{userName}] Slettede {logStringAdder} med ID: {id}.");
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", userName, "deleted");

            return NoContent();
        }
    }
}
