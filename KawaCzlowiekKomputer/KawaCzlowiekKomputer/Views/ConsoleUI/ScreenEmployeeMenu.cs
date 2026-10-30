using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenEmployeeMenu : IViewEmployeeMenu
    {
        private static readonly (string Text, EmployeeMenuChoice Choice)[] Options =
        {
            ("Stanowiska",    EmployeeMenuChoice.Stations),
            ("Rezerwacje",    EmployeeMenuChoice.Reservations),
            ("Menu kawiarni", EmployeeMenuChoice.CafeMenu),
            ("Cennik",        EmployeeMenuChoice.Prices),
            ("Karnety",       EmployeeMenuChoice.Subscriptions),
            ("Wyloguj",       EmployeeMenuChoice.Logout)
        };


        public EmployeeMenuChoice Show(string userName)
        {
            int selected = 0;

            // pętla przerysowuje cały ekran po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                WriteCentered("PANEL PRACOWNIKA", 2, Colors.Accent);
                WriteCentered($"Zalogowano jako: {userName}", 3, Colors.Hint);

                int menuTop = 6;

                new Legend(("[↑/↓]", "Wybór"), ("[Enter]", "Zatwierdź"), ("[Esc]", "Wyloguj")).Draw();

                var menu = new MenuArrow(Options.Select(o => o.Text), -1, menuTop, selected);
                int? result = menu.Show();

                if (menu.Resized)
                {
                    selected = menu.Selected; // po przerysowaniu zostaje ten sam wybór
                    continue;
                }

                // Esc traktujemy jako wylogowanie
                return result is null ? EmployeeMenuChoice.Logout : Options[result.Value].Choice;
            }
        }

        private static void WriteCentered(string text, int row, ConsoleColor color)
        {
            int left = Math.Max(0, (ConsoleHelper.WindowWidth - text.Length) / 2);
            Console.SetCursorPosition(left, row);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}