using EFCore_Inheritance_Demo_Main9.Data;
using EFCore_Inheritance_Demo_Main9.Hubs;
using EFCore_Inheritance_Demo_Main9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Linq; // Sørg for at denne er inkluderet til FirstOrDefault

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFCore_Inheritance_Demo_Main9.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarMetadataController : ControllerBase
    {
        private DataContext _context;
        private readonly ILogger<CarMetadataController> _logger;
        private readonly IHubContext<TodoHub> _hubContext;

        public CarMetadataController(DataContext context, ILogger<CarMetadataController> logger, IHubContext<TodoHub> hubContext)
        {
            this._context = context;
            this._logger = logger;
            this._hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetHierarchyMetadata()
        {
            var baseType = typeof(TPTCar);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                // Filtrer assemblies for kun at inkludere dit eget projekt
                .Where(s => s.FullName.StartsWith("EFCore-Inheritance-Demo-Main9"))
                .SelectMany(s =>
                {
                    try
                    {
                        // Forsøg at hente typerne, men fang specifikke fejl
                        return s.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        // Logging for at finde det problematiske assembly
                        _logger.LogError(ex, $"Could not load all types from assembly: {s.FullName}. Returning partial list.");
                        return ex.Types.Where(t => t != null);
                    }
                    catch (Exception ex)
                    {
                        // Fang andre generiske undtagelser
                        _logger.LogError(ex, $"An error occurred while inspecting assembly: {s.FullName}");
                        return Enumerable.Empty<Type>();
                    }
                })
                .Where(p => baseType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .ToList();

            var classMetadataList = new List<object>();
            foreach (var type in types)
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(p => p.DeclaringType == type)
                                    .Select(p => new
                                    {
                                        name = char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1),
                                        type = GetJavaScriptType(p.PropertyType)
                                    })
                                    .ToList();

                var baseClass = type.BaseType != typeof(object) ? type.BaseType?.Name : null;
                var discriminatorValue = type.Name;

                classMetadataList.Add(new
                {
                    name = type.Name,
                    baseClass = baseClass,
                    discriminator = discriminatorValue,
                    properties = properties
                });
            }

            return Ok(new { classes = classMetadataList });
        }

        private string GetJavaScriptType(Type type)
        {
            if (type == typeof(string)) return "string";
            if (type == typeof(int) || type == typeof(double) || type == typeof(decimal)) return "number";
            if (type == typeof(bool)) return "boolean";
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetJavaScriptType(Nullable.GetUnderlyingType(type));
            }
            return "object";
        }
    }
}