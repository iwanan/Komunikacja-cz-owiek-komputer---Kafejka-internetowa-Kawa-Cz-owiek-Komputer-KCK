using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenMainMenu : IViewMainMenu
    {
        private static readonly string[] Logo =
        {
            @" _  __   ____  _  __ ",
            @"| |/ /  / ___|| |/ / ",
            @"| ' /  | |    | ' /  ",
            @"| . \  | |___ | . \  ",
            @"|_|\_\  \____||_|\_\ "
        };

        private static readonly (string Text, MainMenuChoice Choice)[] Options =
        {
            ("Logowanie klienta",   MainMenuChoice.Login),
            ("Rejestracja konta",   MainMenuChoice.Register),
            ("Logowanie pracownika", MainMenuChoice.EmployeeLogin),
            ("Regulamin",           MainMenuChoice.Rules),
            ("Wyjście",             MainMenuChoice.Exit)
        };

        private int _bottomRow;

          public MainMenuChoice Show()
        {
            int selected = 0;

            // pętla przerysowuje cały ekran po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                int row = 1;
                foreach (string line in Logo)
                    WriteCentered(line, row++, Colors.Title);

                row++;
                WriteCentered("KAWA · CZŁOWIEK · KOMPUTER", row++, Colors.Accent);
                WriteCentered("Kafejka internetowa", row++, Colors.Hint);

                row += 2;
                int menuTop = row;
                _bottomRow = menuTop + Options.Length + 1;

                new Legend(("[↑/↓]", "Wybór"), ("[Enter]", "Zatwierdź"), ("[Esc]", "Wyjście")).Draw();

                var menu = new MenuArrow(Options.Select(o => o.Text), -1, menuTop, selected);
                int? result = menu.Show();

                if (menu.Resized)
                {
                    selected = menu.Selected; // po przerysowaniu zostaje ten sam wybór
                    continue;
                }

                // Esc traktujemy jako wyjście z programu
                return result is null ? MainMenuChoice.Exit : Options[result.Value].Choice;
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
