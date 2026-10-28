using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenLogin : IViewLogin
    {
        public (string Login, string Password)? Show(string title, string? error, string login)
        {
            var form = new Form(title, new[] { ("Login", false), ("Hasło", true) }, error, new[] { login, "" });
            string[]? values = form.Show();
            return values is null ? null : (values[0], values[1]);
        }

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}