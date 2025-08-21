using System.Text.Json.Serialization;

namespace EFCore_Inheritance_Demo_Main9.Models
{
    [JsonDerivedType(typeof(TPTCar), "car")]
    [JsonDerivedType(typeof(TPTCarModel), "carModel")]
    public class TPTCar
    {
        public long Id { get; set; }
        public string ?carName { get; set; }
        public int carYear { get; set; }
        public int carPrice { get; set; }
    }
}
