using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenEditSubs : IViewEditSubs
    {
        public int? PickSub(List<Subscription> subscriptions)
        {
            var rows = subscriptions
                .Select(s => new[] { s.Name, $"{s.Hours} h", $"{s.ValidDays} dni", $"{s.Price:0.00} zł" })
                .ToList();
            rows.Add(new[] { "+ Nowy karnet", "", "", "" });

            return Picker.FromTable("Edycja karnetów", new[] { "Karnet", "Godziny", "Ważność", "Cena" }, rows,
                                    rowColor: i => i == subscriptions.Count ? Colors.Accent : Colors.Text,
                                    hint: "Enter otwiera karnet, ostatni wiersz dodaje nowy");
        }

        public EditAction PickAction(string name)
        {
            int? result = Picker.FromMenu(name, "Wybierz, co zrobić", new[] { "Edytuj", "Usuń", "Wróć" });

            return result switch
            {
                0 => EditAction.Edit,
                1 => EditAction.Delete,
                _ => EditAction.Back // Esc albo "Wróć"
            };
        }

        public (string Name, string Hours, string ValidDays, string Price)? Ask(
            string title, string? error, string[]? initial)
        {
            var form = new Form(
                title,
                new[]
                {
                    ("Nazwa", false),
                    ("Liczba godzin", false),
                    ("Ważność w dniach", false),
                    ("Cena w zł (np. 65,00)", false)
                },
                error,
                initial);

            string[]? v = form.Show();
            return v is null ? null : (v[0], v[1], v[2], v[3]);
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}