using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenEditPrices : IViewEditPrices
    {
        public int? PickPrice(List<PriceListing> prices)
        {
            var rows = prices
                .Select(p => new[] { p.Name, $"{p.PricePerHour:0.00} zł / h", p.Description })
                .ToList();
            rows.Add(new[] { "+ Nowa pozycja", "", "" });

            return Picker.FromTable("Edycja cennika", new[] { "Typ", "Cena", "Opis" }, rows,
                                    rowColor: i => i == prices.Count ? Colors.Accent : Colors.Text,
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

        public (string Name, string Price, string Description)? Ask(string title, string? error, string[]? initial)
        {
            var form = new Form(
                title,
                new[]
                {
                    ("Nazwa", false),
                    ("Cena za godzinę w zł (np. 12,50)", false),
                    ("Opis", false)
                },
                error,
                initial);

            string[]? v = form.Show();
            return v is null ? null : (v[0], v[1], v[2]);
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}