namespace KawaCzlowiekKomputer.Models
{
    public class Station : IHasId
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Cpu { get; set; } = string.Empty;
        public string Gpu { get; set; } = string.Empty;
        public string Ram { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
    }
}