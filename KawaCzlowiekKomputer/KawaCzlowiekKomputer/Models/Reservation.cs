namespace KawaCzlowiekKomputer.Models
{
    public class Reservation : IHasId
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int StationId { get; set; }
        public DateTime Start { get; set; }
        public int Hours { get; set; }
    }
}