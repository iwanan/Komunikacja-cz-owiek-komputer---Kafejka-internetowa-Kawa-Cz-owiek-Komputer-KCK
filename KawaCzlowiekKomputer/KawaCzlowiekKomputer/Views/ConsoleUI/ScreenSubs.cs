using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenSubs : IViewSubs
    {
        public void Show(List<Subscription> subscriptions)
        {
            var rows = subscriptions
                .Select(s => new[] { s.Name, $"{s.Hours} h", $"{s.ValidDays} dni", $"{s.Price:0.00} zł" })
                .ToList();

            string hint = rows.Count == 0 ? "Brak dostępnych karnetów." : "Karnety do kupienia przy kasie";
            Picker.FromTable("Karnety", new[] { "Karnet", "Godziny", "Ważność", "Cena" }, rows,
                             hint: hint, readOnly: true);
        }
    }
}