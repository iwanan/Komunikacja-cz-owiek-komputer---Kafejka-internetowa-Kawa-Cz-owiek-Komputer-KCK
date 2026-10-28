namespace KawaCzlowiekKomputer.Models
{
    public class Game : IHasId
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Pegi { get; set; }
        public string AvailableOn { get; set; } = string.Empty;
    }
}