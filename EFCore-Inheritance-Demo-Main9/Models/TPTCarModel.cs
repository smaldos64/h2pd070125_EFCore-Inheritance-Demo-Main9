using System.Text.Json.Serialization;

namespace EFCore_Inheritance_Demo_Main9.Models
{
    [JsonDerivedType(typeof(TPTCarModel), "carModel")]
    public class TPTCarModel : TPTCar
    {
        public string  ?carModel { get; set; }
    }
}
