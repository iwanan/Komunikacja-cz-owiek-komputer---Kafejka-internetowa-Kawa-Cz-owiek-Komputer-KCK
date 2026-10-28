namespace KawaCzlowiekKomputer.Views
{
    public interface IViewRegistration
    {
        // pokazuje formularz, zwraca dane albo null po Esc
        (string Login, string Email, string Password, string Confirm)? Show(string? error, string login, string email);

        // pokazuje komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}