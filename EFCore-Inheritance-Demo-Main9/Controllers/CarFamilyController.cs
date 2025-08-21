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

        //[HttpPost("create-car")]
        //public async Task<IActionResult> CreateCar([FromBody] TPTCar car)
        //{
        //    await _context.TPTCars.AddAsync(car);
        //    await _context.SaveChangesAsync();
        //    return StatusCode(201);
        //}

        //[HttpPost("create-car-model")]
        //public async Task<IActionResult> CreateCarModel([FromBody] TPTCarModel carModel)
        //{
        //    await _context.TPTCarModels.AddAsync(carModel);
        //    await _context.SaveChangesAsync();
        //    return StatusCode(201);
        //}

        //[HttpPut("update-car")]
        //public async Task<IActionResult> UpdateCar(int id, [FromBody] TPTCar car)
        //{
        //    if (id != car.Id)
        //    {
        //        return BadRequest();
        //    }
        //    var carFromDatabase = await _context.TPTCars.FindAsync(id);

        //    if (null == carFromDatabase)
        //    {
        //        return NotFound();
        //    }

        //    carFromDatabase.carPrice = car.carPrice;
        //    carFromDatabase.carYear = car.carYear;
        //    carFromDatabase.carName = car.carName;

        //    _context.TPTCars.Update(carFromDatabase);
        //    await _context.SaveChangesAsync();
        //    return StatusCode(201);
        //}

        //[HttpPut("update-carModel")]
        //public async Task<IActionResult> UpdateCarModel(int id, [FromBody] TPTCarModel carModel)
        //{
        //    if (id != carModel.Id)
        //    {
        //        return BadRequest();
        //    }
        //    var carModelFromDatabase = await _context.TPTCarModels.FindAsync(id);

        //    if (null == carModelFromDatabase)
        //    {
        //        return NotFound();
        //    }

        //    carModelFromDatabase.carPrice = carModel.carPrice;
        //    carModelFromDatabase.carYear = carModel.carYear;
        //    carModelFromDatabase.carName = carModel.carName;
        //    carModelFromDatabase.carModel = carModel.carModel;

        //    _context.TPTCars.Update(carModelFromDatabase);
        //    await _context.SaveChangesAsync();
        //    return StatusCode(201);
        //}

        //[Route("get-cars")]
        //[HttpGet]
        //public async Task<IActionResult> GetCars()
        //{
        //    var cars = await _context.TPTCars.ToListAsync();
        //    return Ok(cars);
        //}

        //[Route("get-carModels")]
        //[HttpGet]
        //public async Task<IActionResult> GetCarModels()
        //{
        //    var carModels = await _context.TPTCarModels.ToListAsync();
        //    return Ok(carModels);
        //}

        //[Route("get-car/{id}")]
        //[HttpGet]
        //public async Task<IActionResult> GetCar(long id)
        //{
        //    var car = await _context.TPTCars.FindAsync(id);

        //    if (null == car)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(car);
        //}

        //[Route("get-carModel/{id}")]
        //[HttpGet]
        //public async Task<IActionResult> GetCarModel(long id)
        //{
        //    var carModel = await _context.TPTCarModels.FindAsync(id);

        //    if (null == carModel)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(carModel);
        //}

        //[HttpDelete("delete-car/{id}")]
        //public async Task<IActionResult> DeleteCar(long id)
        //{
        //    TPTCar tPTCar = await _context.FindAsync<TPTCar>(id);

        //    if (null == tPTCar)
        //    {
        //        return NotFound();
        //    }

        //    _context.Remove<TPTCar>(tPTCar);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        //[HttpDelete("delete-carModel/{id}")]
        //public async Task<IActionResult> DeleteCarModel(long id)
        //{
        //    TPTCarModel tPTCarModel = await _context.FindAsync<TPTCarModel>(id);

        //    if (null == tPTCarModel)
        //    {
        //        return NotFound();
        //    }

        //    _context.Remove<TPTCarModel>(tPTCarModel);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        //[HttpDelete("delete-tpt-user/{id}")]
        //public async Task<IActionResult> DeleteTPTUser(int id)
        //{
        //    TPTUser tptUser = await _context.FindAsync<TPTUser>(id);

        //    if (null == tptUser)
        //    {
        //        return NotFound();
        //    }

        //    _context.Remove<TPTUser>(tptUser);
        //    _context.SaveChanges();

        //    return NoContent();
        //}
    }
}
