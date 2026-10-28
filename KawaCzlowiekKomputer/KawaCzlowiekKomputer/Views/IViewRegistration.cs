namespace KawaCzlowiekKomputer.Views
{
    public interface IViewRegistration
    {
        // pokazuje formularz, zwraca dane albo null po Esc
        (string Login, string Email, string BirthDate, string Password, string Confirm)? Show(
            string? error, string login, string email, string birthDate);

        // pokazuje komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}