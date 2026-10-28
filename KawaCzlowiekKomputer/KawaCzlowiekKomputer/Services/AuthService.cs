using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class AuthService
    {
        private readonly IRepo<Client> _clients;
        private readonly IRepo<Employee> _employees;

        public AuthService(IRepo<Client> clients, IRepo<Employee> employees)
        {
            _clients = clients;
            _employees = employees;
            SeedEmployee();
        }

        // rejestracja klienta: walidacja, unikalność loginu, hash hasła
        public (bool Success, string Message) Register(string login, string email, string password)
        {
            login = login.Trim();
            email = email.Trim();

            string? error = Validation.ValidateLogin(login)
                            ?? Validation.ValidateEmail(email)
                            ?? Validation.ValidatePassword(password);
            if (error is not null) return (false, error);

            if (LoginTaken(login)) return (false, "Ten login jest już zajęty.");

            var (hash, salt) = PasswordHelper.CreateHash(password);
            _clients.Add(new Client { Login = login, Email = email, PasswordHash = hash, Salt = salt });
            return (true, "Konto zostało utworzone.");
        }

        // zwraca klienta albo null, gdy login lub hasło się nie zgadza
        public Client? LoginClient(string login, string password)
        {
            Client? client = _clients.GetAll().FirstOrDefault(c => SameLogin(c.Login, login));
            return client is not null && PasswordHelper.Verify(password, client.PasswordHash, client.Salt)
                ? client : null;
        }

        // zwraca pracownika albo null
        public Employee? LoginEmployee(string login, string password)
        {
            Employee? employee = _employees.GetAll().FirstOrDefault(e => SameLogin(e.Login, login));
            return employee is not null && PasswordHelper.Verify(password, employee.PasswordHash, employee.Salt)
                ? employee : null;
        }

        private bool LoginTaken(string login) =>
            _clients.GetAll().Any(c => SameLogin(c.Login, login)) ||
            _employees.GetAll().Any(e => SameLogin(e.Login, login));

        private static bool SameLogin(string a, string b) =>
            string.Equals(a, b.Trim(), StringComparison.OrdinalIgnoreCase);

        // pierwsze uruchomienie: dodaje konto startowe do Employees.json
        private void SeedEmployee()
        {
            if (_employees.GetAll().Count > 0) return;

            var (hash, salt) = PasswordHelper.CreateHash("admin1");
            _employees.Add(new Employee { Login = "admin", Email = "admin@kck.pl", PasswordHash = hash, Salt = salt });
        }
    }
}