using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenGames : IViewGames
    {
        public void Show(List<Game> games)
        {
            var rows = games
                .Select(g => new[] { g.Title, g.Genre, $"PEGI {g.Pegi}", g.AvailableOn })
                .ToList();

            string hint = rows.Count == 0 ? "Brak gier na liście." : "Gry zainstalowane na stanowiskach";
            Picker.FromTable("Gry", new[] { "Tytuł", "Gatunek", "Wiek", "Stanowiska" }, rows,
                             hint: hint, readOnly: true);
        }
    }
}