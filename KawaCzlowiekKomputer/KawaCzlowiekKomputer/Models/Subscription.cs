namespace KawaCzlowiekKomputer.Models
{
    public class Subscription : IHasId
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Hours { get; set; }
        public int ValidDays { get; set; }
        public decimal Price { get; set; }
    }
}