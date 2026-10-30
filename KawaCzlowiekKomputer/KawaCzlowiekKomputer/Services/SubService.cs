using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class SubService
    {
        private readonly IRepo<Subscription> _subs;

        public SubService(IRepo<Subscription> subs) => _subs = subs;

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