using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class AuthController
    {
        private readonly AuthService _auth;
        private readonly IViewLogin _viewLogin;
        private readonly IViewRegistration _viewRegistration;

        // zalogowany klient, null gdy nikt nie jest zalogowany
        public Client? CurrentClient { get; private set; }

        public AuthController(AuthService auth, IViewLogin viewLogin, IViewRegistration viewRegistration)
        {
            _auth = auth;
            _viewLogin = viewLogin;
            _viewRegistration = viewRegistration;
        }

        // pokazuje formularz do skutku: poprawne dane albo Esc
        public bool LoginClient()
        {
            string? error = null;
            string login = "";

            while (true)
            {
                var data = _viewLogin.Show("Logowanie klienta", error, login);
                if (data is null) return false;

                login = data.Value.Login;
                Client? client = _auth.LoginClient(login, data.Value.Password);
                if (client is not null)
                {
                    CurrentClient = client;
                    return true;
                }

                error = "Nieprawidłowy login lub hasło.";
            }
        }

        public bool LoginEmployee()
        {
            string? error = null;
            string login = "";

            while (true)
            {
                var data = _viewLogin.Show("Logowanie pracownika", error, login);
                if (data is null) return false;

                login = data.Value.Login;
                Employee? employee = _auth.LoginEmployee(login, data.Value.Password);
                if (employee is not null)
                {
                    // panel pracownika dojdzie w kolejnych krokach
                    _viewLogin.ShowMessage($"Zalogowano jako pracownik {employee.Login}. Panel pracownika będzie dostępny wkrótce.", true);
                    return true;
                }

                error = "Nieprawidłowy login lub hasło.";
            }
        }

        public void Register()
        {
            string? error = null;
            string login = "";
            string email = "";
            string birthDate = "";

            while (true)
            {
                var data = _viewRegistration.Show(error, login, email, birthDate);
                if (data is null) return;

                login = data.Value.Login;
                email = data.Value.Email;
                birthDate = data.Value.BirthDate;

                if (data.Value.Password != data.Value.Confirm)
                {
                    error = "Hasła nie są takie same.";
                    continue;
                }

                var (success, message) = _auth.Register(login, email, birthDate, data.Value.Password);
                if (success)
                {
                    _viewRegistration.ShowMessage(message, true);
                    return;
                }

                error = message;
            }
        }

        public void Logout() => CurrentClient = null;
    }
}