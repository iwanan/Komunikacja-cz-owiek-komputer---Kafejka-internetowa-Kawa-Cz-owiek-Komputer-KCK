using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenMenu : IViewMenu
    {
        public int? Pick(List<Offer<MenuItem>> items)
        {
            var rows = items
                .Select(o => new[]
                {
                    o.Item.Name,
                    o.Item.Category,
                    $"{o.Item.Price:0.00} zł",
                    !o.Item.AgeRestricted ? "" : o.IsAllowed ? "18+" : "18+ niedostępne"
                })
                .ToList();

            string hint = rows.Count == 0
                ? "Menu jest puste."
                : items.Any(o => !o.IsAllowed)
                    ? "Enter zamawia, szare pozycje 18+ są niedostępne"
                    : "Enter zamawia wybraną pozycję";

            return Picker.FromTable("Menu kawiarni", new[] { "Pozycja", "Kategoria", "Cena", "Uwagi" }, rows,
                                    rowColor: i => items[i].IsAllowed ? Colors.Text : Colors.Hint,
                                    canPick: i => items[i].IsAllowed,
                                    hint: hint, enterAction: "Zamów");
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}