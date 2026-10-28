using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    // gotowe ekrany wyboru z listy (tabela albo menu) z obsługą zmiany rozmiaru okna
    public static class Picker
    {
        // tabela w ramce, zwraca indeks wybranego wiersza albo null po Esc
        // canPick pozwala zablokować wybór niektórych wierszy (np. zajętych)
        // readOnly = tylko przeglądanie, Enter nic nie robi
        public static int? FromTable(string title, string[] headers, List<string[]> rows,
            Func<int, ConsoleColor>? rowColor = null, Func<int, bool>? canPick = null,
            string? hint = null, string enterAction = "Wybierz", bool readOnly = false)
        {
            int selected = 0;

            // pętla przerysowuje cały ekran po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                int top = 1;
                int tableWidth = new Table(headers, rows, 0, 0).Width; // tylko pomiar szerokości
                int width = Math.Max(Math.Max(tableWidth + 4, (hint?.Length ?? 0) + 6), title.Length + 6);
                int left = Math.Max(0, (ConsoleHelper.WindowWidth - width) / 2);

                var table = new Table(headers, rows, left + (width - tableWidth) / 2, top + 3, selected)
                {
                    RowColor = rowColor
                };

                // statyczne elementy rysujemy raz
                Border.Draw(left, top, width, rows.Count + 6, title: title);
                if (hint is not null) WriteAt(left + 3, top + 1, hint, Colors.Accent);

                if (readOnly)
                    new Legend(("[↑/↓]", "Przewijanie"), ("[Esc]", "Wróć")).Draw();
                else
                    new Legend(("[↑/↓]", "Wybór"), ("[Enter]", enterAction), ("[Esc]", "Wróć")).Draw();

                while (true)
                {
                    int? result = table.Show();

                    if (table.Resized)
                    {
                        selected = table.Selected; // po przerysowaniu zostaje ten sam wybór
                        break;
                    }

                    if (result is null) return null;
                    if (readOnly) continue; // Enter w trybie podglądu nic nie robi
                    if (canPick is null || canPick(result.Value)) return result;

                    // wiersz zablokowany, czekamy dalej
                }
            }
        }

        // menu w ramce, zwraca indeks pozycji albo null po Esc
        public static int? FromMenu(string title, string? hint, string[] items)
        {
            int selected = 0;

            // pętla przerysowuje cały ekran po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                int top = 1;
                int menuWidth = items.Max(i => i.Length) + 4;
                int width = Math.Max(Math.Max(menuWidth, hint?.Length ?? 0), title.Length) + 8;
                int left = Math.Max(0, (ConsoleHelper.WindowWidth - width) / 2);

                // statyczne elementy rysujemy raz
                Border.Draw(left, top, width, items.Length + 5, title: title);
                if (hint is not null) WriteAt(left + 3, top + 1, hint, Colors.Accent);
                new Legend(("[↑/↓]", "Wybór"), ("[Enter]", "Zatwierdź"), ("[Esc]", "Wróć")).Draw();

                var menu = new MenuArrow(items, left + (width - menuWidth) / 2, top + 3, selected);
                int? result = menu.Show();

                if (menu.Resized)
                {
                    selected = menu.Selected; // po przerysowaniu zostaje ten sam wybór
                    continue;
                }

                return result;
            }
        }

        private static void WriteAt(int left, int row, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(left, row);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}