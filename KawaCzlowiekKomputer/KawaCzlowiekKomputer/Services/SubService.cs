using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class SubService
    {
        private readonly IRepo<Subscription> _subs;
        private readonly IRepo<ClientSubscription> _owned;

        public SubService(IRepo<Subscription> subs, IRepo<ClientSubscription> owned)
        {
            _subs = subs;
            _owned = owned;
        }

        // nadaje klientowi karnet po zakupie, ważność liczona od dziś (włącznie z ostatnim dniem)
        public ClientSubscription Grant(int clientId, Subscription offer)
        {
            var owned = new ClientSubscription
            {
                ClientId = clientId,
                Name = offer.Name,
                TotalHours = offer.Hours,
                HoursLeft = offer.Hours,
                PurchaseDate = DateTime.Now,
                ExpiresOn = DateTime.Today.AddDays(offer.ValidDays)
            };
            _owned.Add(owned);
            return owned;
        }

        // wszystkie karnety klienta, aktywne na górze (najszybciej wygasające), potem pozostałe
        public List<ClientSubscription> GetForClient(int clientId) =>
            _owned.GetAll()
                .Where(c => c.ClientId == clientId)
                .OrderByDescending(c => c.IsActive)
                .ThenBy(c => c.ExpiresOn)
                .ThenBy(c => c.Id)
                .ToList();

        // suma godzin z aktywnych karnetów
        public int GetHoursLeft(int clientId) =>
            _owned.GetAll().Where(c => c.ClientId == clientId && c.IsActive).Sum(c => c.HoursLeft);

        // karnet, który pokryje całą rezerwację: ważny w dniu rezerwacji i z wystarczającą liczbą godzin,
        // najpierw ten, który wygaśnie najszybciej
        public ClientSubscription? FindForReservation(int clientId, DateTime start, int hours) =>
            _owned.GetAll()
                .Where(c => c.ClientId == clientId && c.HoursLeft >= hours && c.IsValidOn(start))
                .OrderBy(c => c.ExpiresOn)
                .ThenBy(c => c.Id)
                .FirstOrDefault();

        // odlicza godziny, false gdy karnet nie istnieje, jest cudzy, za mały albo wygasa przed terminem
        public bool TryUse(int clientSubscriptionId, int clientId, DateTime start, int hours)
        {
            ClientSubscription? c = _owned.GetById(clientSubscriptionId);
            if (c is null || c.ClientId != clientId || c.HoursLeft < hours || !c.IsValidOn(start))
                return false;

            c.HoursLeft -= hours;
            _owned.Update(c);
            return true;
        }

        // zwraca godziny po anulowaniu rezerwacji, nigdy powyżej pierwotnej liczby godzin karnetu
        public void Refund(int clientSubscriptionId, int hours)
        {
            ClientSubscription? c = _owned.GetById(clientSubscriptionId);
            if (c is null) return;

            c.HoursLeft = Math.Min(c.TotalHours, c.HoursLeft + hours);
            _owned.Update(c);
        }

        // karnety w kolejności Id
        public List<Subscription> GetSubscriptions() => _subs.GetAll().OrderBy(s => s.Id).ToList();

        public (bool Success, string Message) Add(string name, string hours, string validDays, string price)
        {
            string? error = Validate(0, name, hours, validDays, price, out int h, out int d, out decimal p);
            if (error is not null) return (false, error);

            _subs.Add(new Subscription { Name = name.Trim(), Hours = h, ValidDays = d, Price = p });
            return (true, "Karnet został dodany.");
        }

        public (bool Success, string Message) Update(int id, string name, string hours, string validDays, string price)
        {
            Subscription? sub = _subs.GetById(id);
            if (sub is null) return (false, "Nie znaleziono karnetu.");

            string? error = Validate(id, name, hours, validDays, price, out int h, out int d, out decimal p);
            if (error is not null) return (false, error);

            sub.Name = name.Trim();
            sub.Hours = h;
            sub.ValidDays = d;
            sub.Price = p;
            _subs.Update(sub);
            return (true, "Zmiany zostały zapisane.");
        }

        public void Delete(int id) => _subs.Delete(id);

        // zwraca opis błędu albo null, id = 0 dla nowego karnetu
        private string? Validate(int id, string name, string hours, string validDays, string price,
                                 out int h, out int d, out decimal p)
        {
            h = 0;
            d = 0;
            p = 0;

            if (name.Trim().Length == 0) return "Podaj nazwę karnetu.";

            string? error = Validation.ValidatePositiveInt(hours, "Liczba godzin", out h)
                            ?? Validation.ValidatePositiveInt(validDays, "Ważność w dniach", out d)
                            ?? Validation.ValidatePrice(price, out p);
            if (error is not null) return error;

            bool taken = _subs.GetAll().Any(s =>
                s.Id != id && s.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
            return taken ? "Karnet o tej nazwie już istnieje." : null;
        }
    }
}