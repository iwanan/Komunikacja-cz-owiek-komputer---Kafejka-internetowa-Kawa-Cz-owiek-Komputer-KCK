using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenRules : IViewRules
    {
        private const int DetailLines = 4; // ile linii treści punktu widać naraz

        private List<Rules> _rules = new();
        private int _selected;
        private int _left;
        private int _width;
        private int _listTop;
        private int _detailTop;

        public void Show(List<Rules> rules)
        {
            _rules = rules;
            _selected = 0;

            ConsoleHelper.TryClear(); // jedyne czyszczenie, przy wejściu na ekran

            _width = Math.Clamp(ConsoleHelper.WindowWidth - 4, 40, 76);
            _left = Math.Max(0, (ConsoleHelper.WindowWidth - _width) / 2);

            int top = 1;
            _listTop = top + 2;
            int separatorRow = _listTop + Math.Max(rules.Count, 1) + 1;
            _detailTop = separatorRow + 1;
            int bottomRow = _detailTop + DetailLines + 1;

            // statyczne elementy rysujemy raz
            Border.Draw(_left, top, _width, bottomRow - top + 1, title: "Regulamin");

            if (rules.Count == 0)
            {
                WriteAt(_left + 2, _listTop, "Brak punktów regulaminu.", Colors.Warning);
                new Legend(("[Esc]", "Wróć")).Draw();
                WaitForEscape();
                return;
            }

            DrawSeparator(separatorRow);
            for (int i = 0; i < rules.Count; i++)
                DrawItem(i);
            DrawDetail();

            new Legend(("[↑/↓]", "Wybór"), ("[Esc]", "Wróć")).Draw();

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        MoveTo((_selected - 1 + _rules.Count) % _rules.Count);
                        break;

                    case ConsoleKey.DownArrow:
                        MoveTo((_selected + 1) % _rules.Count);
                        break;

                    case ConsoleKey.Home:
                        MoveTo(0);
                        break;

                    case ConsoleKey.End:
                        MoveTo(_rules.Count - 1);
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        // przerysowuje tylko dwie pozycje listy i panel szczegółów
        private void MoveTo(int newIndex)
        {
            if (newIndex == _selected) return;

            int old = _selected;
            _selected = newIndex;
            DrawItem(old);
            DrawItem(_selected);
            DrawDetail();
        }

        private void DrawItem(int index)
        {
            bool selected = index == _selected;
            int textWidth = _width - 8; // szerokość pozycji minus " ► " i spacja na końcu

            string text = $"{index + 1}. {_rules[index].Title}";
            if (text.Length > textWidth) text = text[..textWidth];

            Console.SetCursorPosition(_left + 2, _listTop + index);
            Console.ForegroundColor = selected ? Colors.SelectedForeground : Colors.Text;
            Console.BackgroundColor = selected ? Colors.SelectedBackground : ConsoleColor.Black;
            Console.Write($" {(selected ? "►" : " ")} {text.PadRight(textWidth)} ");
            Console.ResetColor();
        }

        private void DrawDetail()
        {
            int w = _width - 4;
            List<string> lines = Wrap(_rules[_selected].Content, w);

            for (int i = 0; i < DetailLines; i++)
            {
                string text = i < lines.Count ? lines[i] : ""; // puste linie czyszczą stary tekst
                if (text.Length > w) text = text[..w];

                Console.SetCursorPosition(_left + 2, _detailTop + i);
                Console.ForegroundColor = Colors.Text;
                Console.Write(text.PadRight(w));
            }
            Console.ResetColor();
        }

        private void DrawSeparator(int row)
        {
            Console.SetCursorPosition(_left, row);
            Console.ForegroundColor = Colors.Border;
            Console.Write("├" + new string('─', _width - 2) + "┤");
            Console.ResetColor();
        }

        // dzieli tekst na linie nie dłuższe niż width, łamiąc między słowami
        private static List<string> Wrap(string text, int width)
        {
            var lines = new List<string>();
            string current = "";

            foreach (string word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (current.Length == 0)
                    current = word;
                else if (current.Length + 1 + word.Length <= width)
                    current += " " + word;
                else
                {
                    lines.Add(current);
                    current = word;
                }
            }

            if (current.Length > 0) lines.Add(current);
            return lines;
        }

        private static void WriteAt(int left, int row, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(left, row);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        private static void WaitForEscape()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
        }
    }
}