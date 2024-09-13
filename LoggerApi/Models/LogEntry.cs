namespace LoggerApi.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string Mpn { get; set; }
        public string Manufacturer { get; set; }
        public Guid MpnGroupGuid { get; set; }
    }
}
