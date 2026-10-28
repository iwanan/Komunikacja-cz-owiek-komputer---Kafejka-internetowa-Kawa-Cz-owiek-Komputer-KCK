namespace KawaCzlowiekKomputer.Views
{
    public interface IViewLogin
    {
        // pokazuje formularz, zwraca login i hasło albo null po Esc
        (string Login, string Password)? Show(string title, string? error, string login);

        // pokazuje komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}