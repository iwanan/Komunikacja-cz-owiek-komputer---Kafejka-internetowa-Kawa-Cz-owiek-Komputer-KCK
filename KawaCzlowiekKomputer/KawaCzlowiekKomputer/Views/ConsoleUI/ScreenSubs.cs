using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenSubs : IViewSubs
    {
        public int? Pick(List<Subscription> subscriptions)
        {
            var rows = subscriptions
                .Select(s => new[] { s.Name, $"{s.Hours} h", $"{s.ValidDays} dni", $"{s.Price:0.00} zł" })
                .ToList();

            string hint = rows.Count == 0 ? "Brak dostępnych karnetów." : "Enter kupuje wybrany karnet";
            return Picker.FromTable("Karnety", new[] { "Karnet", "Godziny", "Ważność", "Cena" }, rows,
                                    hint: hint, enterAction: "Kup");
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}