using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    /// Tabela z wyrównanymi kolumnami, sterowana strzałkami.
    /// Przy ruchu przerysowuje tylko dwa wiersze, bez Console.Clear().
    public class Table
    {
        private readonly string[] _headers;
        private readonly List<string[]> _rows;
        private readonly int[] _widths;
        private readonly int _left;
        private readonly int _top;
        private int _selected = 0;

        // kolor tekstu wiersza o danym indeksie (np. zielony = wolne, czerwony = zajęte)
        public Func<int, ConsoleColor>? RowColor { get; set; }

        // Szerokość tabeli w znakach
        public int Width => _widths.Sum() + 3 * (_widths.Length - 1) + 2;

        // kolumna lewej krawędzi; wartość ujemna = wyśrodkuj
        // wiersz nagłówka. Dane zaczynają się dwa wiersze niżej
        public Table(IEnumerable<string> headers, IEnumerable<string[]> rows, int left, int top)
        {
            _headers = headers.ToArray();
            _rows = rows.ToList();
            _top = top;

            // szerokość kolumny = najdłuższy tekst z nagłówka i danych
            _widths = new int[_headers.Length];
            for (int c = 0; c < _headers.Length; c++)
            {
                int maxData = _rows.Select(r => c < r.Length ? r[c].Length : 0).DefaultIfEmpty(0).Max();
                _widths[c] = Math.Max(_headers[c].Length, maxData);
            }

            _left = left < 0 ? Math.Max(0, (ConsoleHelper.WindowWidth - Width) / 2) : left;
        }

        // Rysuje tabelę, czeka na wybór. Zwraca indeks wiersza albo null po Esc
        public int? Show()
        {
            DrawHeader();
            for (int i = 0; i < _rows.Count; i++)
                DrawRow(i);

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow when _rows.Count > 0:
                        MoveTo((_selected - 1 + _rows.Count) % _rows.Count);
                        break;

                    case ConsoleKey.DownArrow when _rows.Count > 0:
                        MoveTo((_selected + 1) % _rows.Count);
                        break;

                    case ConsoleKey.Home when _rows.Count > 0:
                        MoveTo(0);
                        break;

                    case ConsoleKey.End when _rows.Count > 0:
                        MoveTo(_rows.Count - 1);
                        break;

                    case ConsoleKey.Enter when _rows.Count > 0:
                        return _selected;

                    case ConsoleKey.Escape:
                        return null;
                }
            }
        }

        // podmienia dane jednego wiersza i przerysowuje tylko jego.</summary>
        public void UpdateRow(int index, string[] cells)
        {
            _rows[index] = cells;
            DrawRow(index);
        }

        private void MoveTo(int newIndex)
        {
            if (newIndex == _selected) return;

            int old = _selected;
            _selected = newIndex;
            DrawRow(old);       // zdejmij podświetlenie
            DrawRow(_selected); // załóż podświetlenie
        }

        private void DrawHeader()
        {
            Console.SetCursorPosition(_left, _top);
            Console.ForegroundColor = Colors.Title;
            Console.Write(BuildLine(_headers));

            Console.SetCursorPosition(_left, _top + 1);
            Console.ForegroundColor = Colors.Border;
            Console.Write("─" + string.Join("─┼─", _widths.Select(w => new string('─', w))) + "─");
            Console.ResetColor();
        }

        private void DrawRow(int index)
        {
            bool selected = index == _selected;

            Console.SetCursorPosition(_left, _top + 2 + index);
            Console.ForegroundColor = selected ? Colors.SelectedForeground : (RowColor?.Invoke(index) ?? Colors.Text);
            Console.BackgroundColor = selected ? Colors.SelectedBackground : ConsoleColor.Black;
            Console.Write(BuildLine(_rows[index]));
            Console.ResetColor();
        }

        // składa jedną linię tabeli, każda komórka dopełniona lub ucięta do szerokości kolumny
        private string BuildLine(string[] cells)
        {
            var parts = new string[_widths.Length];
            for (int c = 0; c < _widths.Length; c++)
            {
                string text = c < cells.Length ? cells[c] : "";
                if (text.Length > _widths[c]) text = text[.._widths[c]];
                parts[c] = text.PadRight(_widths[c]);
            }
            return " " + string.Join(" │ ", parts) + " ";
        }
    }
}
