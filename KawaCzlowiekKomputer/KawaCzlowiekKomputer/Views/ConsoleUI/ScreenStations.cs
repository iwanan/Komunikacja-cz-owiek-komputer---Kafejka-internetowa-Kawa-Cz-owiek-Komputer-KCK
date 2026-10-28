using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenStations : IViewStations
    {
        private const int DetailLines = 3; // procesor, karta graficzna, RAM

        private List<Station> _stations = new();
        private int _selected;
        private int _borderLeft;
        private int _borderWidth;
        private int _detailTop;

        public void Show(List<Station> stations)
        {
            _stations = stations;
            _selected = 0;

            // przerysowuje ekran po każdej zmianie rozmiaru okna
            while (DrawAndWait()) { }
        }

        // zwraca true, gdy trzeba przerysować ekran (zmiana rozmiaru), false po Esc
        private bool DrawAndWait()
        {
            ConsoleHelper.TryClear(); // jedyne czyszczenie, przy wejściu na ekran

            int top = 1;

            if (_stations.Count == 0)
            {
                int w = 40;
                int l = Math.Max(0, (ConsoleHelper.WindowWidth - w) / 2);
                Border.Draw(l, top, w, 5, title: "Stanowiska");
                WriteAt(l + 2, top + 2, "Brak stanowisk.", Colors.Warning);
                new Legend(("[Esc]", "Wróć")).Draw();

                while (true)
                {
                    if (!ConsoleHelper.WaitForKey(out ConsoleKeyInfo k)) return true;
                    if (k.Key == ConsoleKey.Escape) return false;
                }
            }

            var rows = _stations
                .Select(s => new[] { s.Name, s.Type, s.IsOccupied ? "● Zajęte" : "● Wolne" })
                .ToList();

            int tableTop = top + 2;
            var table = new Table(new[] { "Stanowisko", "Typ", "Stan" }, rows, -1, tableTop, _selected);
            table.RowColor = i => _stations[i].IsOccupied ? Colors.Taken : Colors.Free;
            table.SelectionChanged = i =>
            {
                _selected = i;
                DrawDetail();
            };

            _borderLeft = Math.Max(0, table.Left - 2);
            _borderWidth = table.Width + 4;

            int separatorRow = tableTop + 2 + _stations.Count;
            _detailTop = separatorRow + 1;
            int bottomRow = _detailTop + DetailLines;

            // statyczne elementy rysujemy raz
            Border.Draw(_borderLeft, top, _borderWidth, bottomRow - top + 1, title: "Stanowiska");
            DrawSeparator(separatorRow);

            int free = _stations.Count(s => !s.IsOccupied);
            WriteAt(_borderLeft + 2, top + 1, $"Wolne: {free} z {_stations.Count}", Colors.Accent);

            DrawDetail();
            new Legend(("[↑/↓]", "Wybór"), ("[Esc]", "Wróć")).Draw();

            while (true)
            {
                int? result = table.Show();

                if (table.Resized)
                {
                    _selected = table.Selected; // po przerysowaniu zostaje ten sam wybór
                    return true;
                }

                if (result is null) return false;

                // Enter: rezerwacja stanowiska dojdzie w kolejnym kroku
            }
        }

        // przerysowuje tylko panel ze specyfikacją zaznaczonego stanowiska
        private void DrawDetail()
        {
            Station s = _stations[_selected];
            string[] lines =
            {
                $"Procesor: {s.Cpu}",
                $"Karta graficzna: {s.Gpu}",
                $"Pamięć RAM: {s.Ram}"
            };

            int w = _borderWidth - 4;
            for (int i = 0; i < DetailLines; i++)
            {
                string text = lines[i];
                if (text.Length > w) text = text[..w];

                Console.SetCursorPosition(_borderLeft + 2, _detailTop + i);
                Console.ForegroundColor = Colors.Text;
                Console.Write(text.PadRight(w)); // pełna szerokość czyści stary tekst
            }
            Console.ResetColor();
        }

        private void DrawSeparator(int row)
        {
            Console.SetCursorPosition(_borderLeft, row);
            Console.ForegroundColor = Colors.Border;
            Console.Write("├" + new string('─', _borderWidth - 2) + "┤");
            Console.ResetColor();
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