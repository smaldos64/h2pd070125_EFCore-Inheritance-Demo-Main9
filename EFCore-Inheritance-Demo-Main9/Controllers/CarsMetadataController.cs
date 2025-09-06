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
    public class CarsMetadataController : ControllerBase
    {
        [HttpGet("hierarchy")]
        public IActionResult GetHierarchyMetadata()
        {
            var baseType = typeof(TPTCar);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                     .SelectMany(s => s.GetTypes())
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

                // Korrekt læsning af baseklassen:
                // Hvis baseClass ikke er en del af vores hierarki (dvs. arver direkte fra 'object'),
                // så skal den vises.
                var baseClass = type.BaseType != typeof(object) ? type.BaseType?.Name : null;

                // Vi bruger stadig en simpel konvention for discriminator-værdien
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
            if (type == typeof(string))
            {
                return "string";
            }

            if (type == typeof(int) || type == typeof(double) || type == typeof(decimal))
            {
                return "number";
            }

            if (type == typeof(bool))
            {
                return "boolean";
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetJavaScriptType(Nullable.GetUnderlyingType(type));
            }

            return "object";
        }
    }
}