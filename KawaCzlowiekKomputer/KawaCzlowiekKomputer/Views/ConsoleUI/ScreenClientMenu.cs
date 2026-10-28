using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenClientMenu : IViewClientMenu
    {
        private static readonly (string Text, ClientMenuChoice Choice)[] Options =
        {
            ("Stanowiska",     ClientMenuChoice.Stations),
            ("Rezerwacje",     ClientMenuChoice.Reservations),
            ("Cennik",         ClientMenuChoice.Prices),
            ("Karnety",        ClientMenuChoice.Subscriptions),
            ("Gry",            ClientMenuChoice.Games),
            ("Menu kawiarni",  ClientMenuChoice.CafeMenu),
            ("Regulamin",      ClientMenuChoice.Rules),
            ("Moje konto",     ClientMenuChoice.Account),
            ("Wyloguj",        ClientMenuChoice.Logout)
        };

        private int _bottomRow;

        public ClientMenuChoice Show(string userName)
        {
            int selected = 0;

            // pętla przerysowuje cały ekran po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                WriteCentered("PANEL KLIENTA", 2, Colors.Accent);
                WriteCentered($"Zalogowano jako: {userName}", 3, Colors.Hint);

                int menuTop = 6;
                _bottomRow = menuTop + Options.Length + 1;

                new Legend(("[↑/↓]", "Wybór"), ("[Enter]", "Zatwierdź"), ("[Esc]", "Wyloguj")).Draw();

                var menu = new MenuArrow(Options.Select(o => o.Text), -1, menuTop, selected);
                int? result = menu.Show();

                if (menu.Resized)
                {
                    selected = menu.Selected; // po przerysowaniu zostaje ten sam wybór
                    continue;
                }

                // Esc traktujemy jako wylogowanie
                return result is null ? ClientMenuChoice.Logout : Options[result.Value].Choice;
            }
        }

        public void ShowMessage(string text)
        {
            int messageRow = _bottomRow + 2;
            WriteCentered(text, messageRow, Colors.Warning);
            WriteCentered("Naciśnij dowolny klawisz...", messageRow + 1, Colors.Hint);
            Console.ReadKey(true);
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