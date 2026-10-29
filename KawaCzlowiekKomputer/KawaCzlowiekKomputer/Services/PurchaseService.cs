using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class PurchaseService
    {
        private readonly IRepo<Purchase> _purchases;

        public PurchaseService(IRepo<Purchase> purchases) => _purchases = purchases;

        // zapisuje zakup z aktualną datą
        public void Add(int clientId, string name, string category, decimal price) =>
            _purchases.Add(new Purchase
            {
                ClientId = clientId,
                Name = name,
                Category = category,
                Price = price,
                Date = DateTime.Now
            });

        // zakupy klienta od najnowszych
        public List<Purchase> GetForClient(int clientId) =>
            _purchases.GetAll()
                .Where(p => p.ClientId == clientId)
                .OrderByDescending(p => p.Date)
                .ToList();

        // suma wydana przez klienta
        public decimal GetTotal(int clientId) => GetForClient(clientId).Sum(p => p.Price);
    }
}