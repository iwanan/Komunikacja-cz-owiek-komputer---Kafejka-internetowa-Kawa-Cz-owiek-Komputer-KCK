using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenEditEmployees : IViewEditEmployees
    {
        public int? PickEmployee(List<Employee> employees, int currentId)
        {
            var rows = employees
                .Select(e => new[] { e.Login, e.Email, e.Id == currentId ? "to Ty" : "" })
                .ToList();
            rows.Add(new[] { "+ Nowy pracownik", "", "" });

            // wybrać można tylko ostatnią pozycję, istniejące konta służą do przeglądania
            return Picker.FromTable("Pracownicy", new[] { "Login", "E-mail", "Uwagi" }, rows,
                                    rowColor: i => i == employees.Count ? Colors.Accent : Colors.Text,
                                    canPick: i => i == employees.Count,
                                    hint: "Enter na ostatniej pozycji dodaje nowego pracownika",
                                    enterAction: "Dodaj");
        }

        public (string Login, string Email, string Password, string Confirm)? Ask(
            string? error, string login, string email)
        {
            var form = new Form(
                "Nowy pracownik",
                new[]
                {
                    ("Login", false),
                    ("E-mail", false),
                    ("Hasło (min. 6 znaków i cyfra)", true),
                    ("Powtórz hasło", true)
                },
                error,
                new[] { login, email, "", "" });

            string[]? v = form.Show();
            return v is null ? null : (v[0], v[1], v[2], v[3]);
        }

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}
