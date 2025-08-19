using System.Text.Json.Serialization;

namespace EFCore_Inheritance_Demo_Main9.Models
{
    [JsonDerivedType(typeof(TPTCar), "car")]
    public class TPTCar
    {
        public long Id { get; set; }
        public string ?carName { get; set; }
        public int carYear { get; set; }
        public int carPrice { get; set; }
    }
}
