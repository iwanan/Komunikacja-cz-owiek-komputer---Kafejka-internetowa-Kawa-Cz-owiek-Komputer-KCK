using System.Globalization;
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

        // data w formacie RRRR-MM-DD, nie z przyszłości i nie starsza niż 120 lat
        public static string? ValidateBirthDate(string text, out DateTime date)
        {
            if (!DateTime.TryParseExact(text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out date))
                return "Podaj datę urodzenia w formacie RRRR-MM-DD.";
            if (date > DateTime.Today)
                return "Data urodzenia nie może być z przyszłości.";
            if (date < DateTime.Today.AddYears(-120))
                return "Podaj prawdziwą datę urodzenia.";
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