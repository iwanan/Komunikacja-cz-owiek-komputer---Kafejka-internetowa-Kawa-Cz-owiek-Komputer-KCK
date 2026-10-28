using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class CafeService
    {
        private const int AdultAge = 18;

        private readonly IRepo<Rules> _rulesRepo;
        private readonly IRepo<PriceListing> _pricesRepo;
        private readonly IRepo<Game> _gamesRepo;
        private readonly IRepo<MenuItem> _menuRepo;

        public CafeService(IRepo<Rules> rulesRepo, IRepo<PriceListing> pricesRepo,
                           IRepo<Game> gamesRepo, IRepo<MenuItem> menuRepo)
        {
            _rulesRepo = rulesRepo;
            _pricesRepo = pricesRepo;
            _gamesRepo = gamesRepo;
            _menuRepo = menuRepo;
        }

        // punkty regulaminu w kolejności Id
        public List<Rules> GetRules() => _rulesRepo.GetAll().OrderBy(r => r.Id).ToList();

        // cennik w kolejności Id
        public List<PriceListing> GetPrices() => _pricesRepo.GetAll().OrderBy(p => p.Id).ToList();

        // wiek w pełnych latach liczony z daty urodzenia
        public static int GetAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--; // urodziny jeszcze w tym roku
            return age;
        }

        // gry alfabetycznie, niedostępne gdy klient jest młodszy niż wymaga PEGI
        public List<Offer<Game>> GetGames(Client client)
        {
            int age = GetAge(client.DateOfBirth);
            return _gamesRepo.GetAll()
                .OrderBy(g => g.Title)
                .Select(g => new Offer<Game> { Item = g, IsAllowed = age >= g.Pegi })
                .ToList();
        }

        // menu w kolejności Id, pozycje 18+ niedostępne dla niepełnoletnich
        public List<Offer<MenuItem>> GetMenu(Client client)
        {
            bool adult = GetAge(client.DateOfBirth) >= AdultAge;
            return _menuRepo.GetAll()
                .OrderBy(m => m.Id)
                .Select(m => new Offer<MenuItem> { Item = m, IsAllowed = !m.AgeRestricted || adult })
                .ToList();
        }
    }
}