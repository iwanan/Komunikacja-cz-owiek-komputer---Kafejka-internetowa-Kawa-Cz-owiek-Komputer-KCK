using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenEditMenu : IViewEditMenu
    {
        public int? PickItem(List<MenuItem> items)
        {
            var rows = items
                .Select(m => new[] { m.Name, m.Category, $"{m.Price:0.00} zł", m.AgeRestricted ? "18+" : "" })
                .ToList();
            rows.Add(new[] { "+ Nowa pozycja", "", "", "" });

            return Picker.FromTable("Edycja menu kawiarni", new[] { "Pozycja", "Kategoria", "Cena", "Uwagi" }, rows,
                                    rowColor: i => i == items.Count ? Colors.Accent : Colors.Text,
                                    hint: "Enter otwiera pozycję, ostatni wiersz dodaje nową");
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

        public (string Name, string Category, string Price, string Restricted)? Ask(
            string title, string? error, string[]? initial)
        {
            var form = new Form(
                title,
                new[]
                {
                    ("Nazwa", false),
                    ("Kategoria", false),
                    ("Cena w zł (np. 12,50)", false),
                    ("Ograniczenie 18+ (tak/nie)", false)
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