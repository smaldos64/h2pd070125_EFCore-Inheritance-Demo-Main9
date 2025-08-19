namespace EFCore_Inheritance_Demo_Main9.Models
{
    public class LogInfo
    {
        public long LogInfoId { get; set; }
        public string? LogInfoUserName { get; set; }
        public string? LogInfoDescription { get; set; }

        public DateTime? LogInfoDateAndTime { get; set; } = DateTime.Now;
    }
}
