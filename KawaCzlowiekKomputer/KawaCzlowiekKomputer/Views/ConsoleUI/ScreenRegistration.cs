using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenRegistration : IViewRegistration
    {
        public (string Login, string Email, string BirthDate, string Password, string Confirm)? Show(
            string? error, string login, string email, string birthDate)
        {
            var form = new Form(
                "Rejestracja konta",
                new[]
                {
                    ("Login", false),
                    ("E-mail", false),
                    ("Data urodzenia (RRRR-MM-DD)", false),
                    ("Hasło (min. 6 znaków i cyfra)", true),
                    ("Powtórz hasło", true)
                },
                error,
                new[] { login, email, birthDate, "", "" });

            string[]? v = form.Show();
            return v is null ? null : (v[0], v[1], v[2], v[3], v[4]);
        }

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}