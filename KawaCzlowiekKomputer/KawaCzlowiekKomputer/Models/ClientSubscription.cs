using System.Text.Json.Serialization;

namespace KawaCzlowiekKomputer.Models
{
    // karnet kupiony przez klienta: kopia danych z oferty (zmiana oferty nie psuje starych karnetów)
    public class ClientSubscription : IHasId
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalHours { get; set; }
        public int HoursLeft { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiresOn { get; set; } // ostatni dzień ważności

        // karnet można użyć na dany dzień, gdy jeszcze nie wygasł
        public bool IsValidOn(DateTime day) => day.Date <= ExpiresOn.Date;

        // aktywny dziś: zostały godziny i nie wygasł (liczone, nie zapisywane w JSON)
        [JsonIgnore]
        public bool IsActive => HoursLeft > 0 && IsValidOn(DateTime.Today);
    }
}
