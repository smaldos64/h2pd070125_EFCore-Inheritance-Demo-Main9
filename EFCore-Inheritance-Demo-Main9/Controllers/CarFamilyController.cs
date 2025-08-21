using EFCore_Inheritance_Demo_Main9.Data;
using EFCore_Inheritance_Demo_Main9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InheritanceStrategyDemoEFCore5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarFamilyController : ControllerBase
    {
        private DataContext _context;
        public CarFamilyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _context.TPTCars.ToListAsync();
            return Ok(cars);
        }

        [HttpGet("GetCarById/{id}")]

        public async Task<IActionResult> GetCarById(long id)
        {
            var car = await _context.TPTCars.FindAsync(id);
            //var car = await _context.TPTCars
            //.OfType<TPTCarModel>() // Inkluderer CarModel-typen
            //.SingleOrDefaultAsync(c => c.Id == id);

            if (car == null) 
            {
                return NotFound();
            }

            // Tving serializeren til at anvende regler for polymorfi
            // ved at lave om til en liste.
            var cars = new List<TPTCar> { car };
            
            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCar([FromBody]TPTCar car)
        {
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

            _context.TPTCars.Add(car);
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] TPTCar car)
        {
            if (id != car.Id)
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
                if (!_context.TPTCars.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(long id)
        {
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

            return NoContent();
        }
    }
}
