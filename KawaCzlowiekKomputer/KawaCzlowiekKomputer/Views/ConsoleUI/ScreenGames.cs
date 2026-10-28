using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenGames : IViewGames
    {
        public void Show(List<Offer<Game>> games)
        {
            var rows = games
                .Select(o => new[]
                {
                    o.Item.Title,
                    o.Item.Genre,
                    $"PEGI {o.Item.Pegi}",
                    o.Item.AvailableOn,
                    o.IsAllowed ? "" : "Za młody wiek"
                })
                .ToList();

            string hint = rows.Count == 0
                ? "Brak gier na liście."
                : games.Any(o => !o.IsAllowed)
                    ? "Szare gry mają ograniczenie wiekowe PEGI"
                    : "Gry zainstalowane na stanowiskach";

            Picker.FromTable("Gry", new[] { "Tytuł", "Gatunek", "Wiek", "Stanowiska", "Dostęp" }, rows,
                             rowColor: i => games[i].IsAllowed ? Colors.Text : Colors.Hint,
                             hint: hint, readOnly: true);
        }
    }
}