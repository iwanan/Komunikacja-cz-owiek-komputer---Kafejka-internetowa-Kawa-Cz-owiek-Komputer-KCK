using System.Text.RegularExpressions;

namespace KawaCzlowiekKomputer.Helpers
{
    // walidacja danych z formularzy, zwraca opis błędu albo null gdy wszystko OK
    public static class Validation
    {
        private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public static string? ValidateLogin(string login)
        {
            if (login.Length < 3 || login.Length > 20)
                return "Login musi mieć od 3 do 20 znaków.";
            if (!login.All(c => char.IsLetterOrDigit(c) || c == '_'))
                return "Login może zawierać tylko litery, cyfry i podkreślenie.";
            return null;
        }

        public static string? ValidateEmail(string email)
        {
            if (!EmailPattern.IsMatch(email))
                return "Podaj poprawny adres e-mail.";
            return null;
        }

        public static string? ValidatePassword(string password)
        {
            if (password.Length < 6)
                return "Hasło musi mieć minimum 6 znaków.";
            if (!password.Any(char.IsDigit))
                return "Hasło musi zawierać co najmniej jedną cyfrę.";
            return null;
        }
    }
}