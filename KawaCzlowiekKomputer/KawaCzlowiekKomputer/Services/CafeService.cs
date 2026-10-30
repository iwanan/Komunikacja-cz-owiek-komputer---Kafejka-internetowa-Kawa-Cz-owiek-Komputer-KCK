using KawaCzlowiekKomputer.Helpers;
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

        // czy klient ma co najmniej 18 lat
        public static bool IsAdult(DateTime birthDate) => GetAge(birthDate) >= AdultAge;

        // menu w kolejności Id, pozycje 18+ niedostępne dla niepełnoletnich
        public List<Offer<MenuItem>> GetMenu(Client client)
        {
            bool adult = GetAge(client.DateOfBirth) >= AdultAge;
            return _menuRepo.GetAll()
                .OrderBy(m => m.Id)
                .Select(m => new Offer<MenuItem> { Item = m, IsAllowed = !m.AgeRestricted || adult })
                .ToList();
        }

        // menu w kolejności Id do edycji, bez filtrowania po wieku
        public List<MenuItem> GetMenuForEdit() => _menuRepo.GetAll().OrderBy(m => m.Id).ToList();

        public (bool Success, string Message) AddMenuItem(string name, string category, string price, string restricted)
        {
            string? error = ValidateMenuItem(0, name, category, price, restricted, out decimal p, out bool r);
            if (error is not null) return (false, error);

            _menuRepo.Add(new MenuItem { Name = name.Trim(), Category = category.Trim(), Price = p, AgeRestricted = r });
            return (true, "Pozycja została dodana.");
        }

        public (bool Success, string Message) UpdateMenuItem(int id, string name, string category, string price, string restricted)
        {
            MenuItem? item = _menuRepo.GetById(id);
            if (item is null) return (false, "Nie znaleziono pozycji.");

            string? error = ValidateMenuItem(id, name, category, price, restricted, out decimal p, out bool r);
            if (error is not null) return (false, error);

            item.Name = name.Trim();
            item.Category = category.Trim();
            item.Price = p;
            item.AgeRestricted = r;
            _menuRepo.Update(item);
            return (true, "Zmiany zostały zapisane.");
        }

        public void DeleteMenuItem(int id) => _menuRepo.Delete(id);

        public (bool Success, string Message) AddPrice(string name, string price, string description)
        {
            string? error = ValidatePrice(0, name, price, description, out decimal p);
            if (error is not null) return (false, error);

            _pricesRepo.Add(new PriceListing { Name = name.Trim(), PricePerHour = p, Description = description.Trim() });
            return (true, "Pozycja cennika została dodana.");
        }

        public (bool Success, string Message) UpdatePrice(int id, string name, string price, string description)
        {
            PriceListing? listing = _pricesRepo.GetById(id);
            if (listing is null) return (false, "Nie znaleziono pozycji.");

            string? error = ValidatePrice(id, name, price, description, out decimal p);
            if (error is not null) return (false, error);

            listing.Name = name.Trim();
            listing.PricePerHour = p;
            listing.Description = description.Trim();
            _pricesRepo.Update(listing);
            return (true, "Zmiany zostały zapisane.");
        }

        public void DeletePrice(int id) => _pricesRepo.Delete(id);

        // zwraca opis błędu albo null, id = 0 dla nowej pozycji
        private string? ValidateMenuItem(int id, string name, string category, string price, string restricted,
                                         out decimal p, out bool r)
        {
            p = 0;
            r = false;

            if (name.Trim().Length == 0 || category.Trim().Length == 0)
                return "Wypełnij nazwę i kategorię.";

            string? error = Validation.ValidatePrice(price, out p);
            if (error is not null) return error;

            string answer = restricted.Trim().ToLowerInvariant();
            if (answer != "tak" && answer != "nie") return "Pole 18+ musi mieć wartość tak lub nie.";
            r = answer == "tak";

            bool taken = _menuRepo.GetAll().Any(m =>
                m.Id != id && m.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
            return taken ? "Pozycja o tej nazwie już istnieje." : null;
        }

        private string? ValidatePrice(int id, string name, string price, string description, out decimal p)
        {
            p = 0;

            if (name.Trim().Length == 0 || description.Trim().Length == 0)
                return "Wypełnij nazwę i opis.";

            string? error = Validation.ValidatePrice(price, out p);
            if (error is not null) return error;

            bool taken = _pricesRepo.GetAll().Any(x =>
                x.Id != id && x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
            return taken ? "Pozycja o tej nazwie już istnieje." : null;
        }
    }
}