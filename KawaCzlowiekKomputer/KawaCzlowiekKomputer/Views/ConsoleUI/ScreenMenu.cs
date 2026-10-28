using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenMenu : IViewMenu
    {
        public void Show(List<Offer<MenuItem>> items)
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
                    ? "Pozycje 18+ są niedostępne dla niepełnoletnich"
                    : "Zamówienia przy barze";

            Picker.FromTable("Menu kawiarni", new[] { "Pozycja", "Kategoria", "Cena", "Uwagi" }, rows,
                             rowColor: i => items[i].IsAllowed ? Colors.Text : Colors.Hint,
                             hint: hint, readOnly: true);
        }
    }
}