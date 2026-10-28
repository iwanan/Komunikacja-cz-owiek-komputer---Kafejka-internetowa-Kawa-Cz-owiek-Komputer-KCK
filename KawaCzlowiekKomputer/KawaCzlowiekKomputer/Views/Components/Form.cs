using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    // formularz z kilkoma polami TextBox, przechodzenie Tab i strzałkami
    public class Form
    {
        private readonly string _title;
        private readonly (string Label, bool Masked)[] _fields;
        private readonly string? _error;
        private readonly string[] _values;

        // initial to wartości startowe pól, np. wpisany wcześniej login
        public Form(string title, (string Label, bool Masked)[] fields, string? error = null, string[]? initial = null)
        {
            _title = title;
            _fields = fields;
            _error = error;

            _values = new string[fields.Length];
            for (int i = 0; i < _values.Length; i++)
                _values[i] = initial is not null && i < initial.Length ? initial[i] : "";
        }

        // zwraca wartości pól po Enter na ostatnim polu albo null po Esc
        public string[]? Show()
        {
            int n = _fields.Length;
            int current = 0;

            // pętla przerysowuje cały ekran po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                int width = Math.Clamp(ConsoleHelper.WindowWidth - 4, 40, 60);
                int left = Math.Max(0, (ConsoleHelper.WindowWidth - width) / 2);
                int top = 2;
                int errorRow = top + 2 + n * 3;

                // statyczne elementy rysujemy raz
                Border.Draw(left, top, width, n * 3 + 4, title: _title);

                var boxes = new TextBox[n];
                for (int i = 0; i < n; i++)
                {
                    int row = top + 2 + i * 3;
                    WriteAt(left + 3, row, _fields[i].Label, Colors.Accent);

                    boxes[i] = new TextBox(left + 3, row + 1, width - 6, _fields[i].Masked);
                    boxes[i].Text = _values[i];
                    boxes[i].Draw(false);
                }

                if (_error is not null)
                {
                    string error = _error.Length > width - 6 ? _error[..(width - 6)] : _error;
                    WriteAt(left + 3, errorRow, error, Colors.Error);
                }

                new Legend(("[Tab/↑/↓]", "Pole"), ("[Enter]", "Dalej"), ("[Esc]", "Wróć")).Draw();

                while (true)
                {
                    ConsoleKey? key = boxes[current].Read();
                    _values[current] = boxes[current].Text;

                    if (key is null) break; // zmiana rozmiaru okna, przerysuj ekran

                    switch (key.Value)
                    {
                        case ConsoleKey.Escape:
                            return null;

                        case ConsoleKey.UpArrow:
                            current = (current - 1 + n) % n;
                            break;

                        case ConsoleKey.Tab:
                        case ConsoleKey.DownArrow:
                            current = (current + 1) % n;
                            break;

                        case ConsoleKey.Enter:
                            if (current == n - 1) return (string[])_values.Clone();
                            current++;
                            break;
                    }
                }
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