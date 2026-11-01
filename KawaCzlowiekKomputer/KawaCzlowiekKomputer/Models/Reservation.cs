namespace KawaCzlowiekKomputer.Models
{
    public class Reservation : IHasId
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int StationId { get; set; }
        public DateTime Start { get; set; }
        public int Hours { get; set; }

        // Id karnetu klienta, z którego odliczono godziny (null = bez karnetu)
        public int? ClientSubscriptionId { get; set; }

        // true, gdy pracownik potwierdził, że klient przyszedł
        public bool CheckedIn { get; set; }
    }
}