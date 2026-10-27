using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    // Menu sterowane strzałkami. Przy zmianie zaznaczenia przerysowuje
    // tylko dwie linie (poprzednią i nową), bez Console.Clear().
    public class MenuArrow
    {
        private readonly string[] _items;
        private readonly int _left;
        private readonly int _top;
        private readonly int _textWidth;
        private int _selected=0;

        // Teksty pozycji
        // Kolumna lewej krawędzi; wartość ujemna = wyśrodkuj w poziomie
        // Wiersz pierwszej pozycji
          public MenuArrow(IEnumerable<string> items, int left, int top, int selected = 0)
        {
            _items = items.ToArray();
            if (_items.Length == 0)
                throw new ArgumentException("Menu musi mieć co najmniej jedną pozycję.");

            _textWidth = _items.Max(i => i.Length);
            _top = top;
            _left = left < 0 ? Math.Max(0, (ConsoleHelper.WindowWidth - Width) / 2) : left;
             _selected = Math.Clamp(selected, 0, _items.Length - 1); // zachowuje wybór po przerysowaniu
        }

        // szerokość narysowanej pozycji menu w znakach
        public int Width => _textWidth + 4;
         // true, gdy Show() przerwano z powodu zmiany rozmiaru okna
        public bool Resized { get; private set; }

        // aktualnie zaznaczona pozycja
        public int Selected => _selected;

        // Pokazuje menu i czeka na wybór
        // Zwraca indeks wybranej pozycji albo null po naciśnięciu Esc
        public int? Show()
        {
            for (int i = 0; i < _items.Length; i++)
                DrawItem(i);

            while (true)
            {
                  if (!ConsoleHelper.WaitForKey(out ConsoleKeyInfo key))
                {
                    Resized = true;
                    return null;
                }

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        MoveTo((_selected - 1 + _items.Length) % _items.Length);
                        break;

                    case ConsoleKey.DownArrow:
                        MoveTo((_selected + 1) % _items.Length);
                        break;

                    case ConsoleKey.Home:
                        MoveTo(0);
                        break;

                    case ConsoleKey.End:
                        MoveTo(_items.Length - 1);
                        break;

                    case ConsoleKey.Enter:
                        return _selected;

                    case ConsoleKey.Escape:
                        return null;
                }
            }
        }

        private void MoveTo(int newIndex)
        {
            if (newIndex == _selected) return;

            int old = _selected;
            _selected = newIndex;
            DrawItem(old);        // zdejmij podświetlenie
            DrawItem(_selected);  // załóż podświetlenie
        }

        private void DrawItem(int index)
        {
            bool selected = index == _selected;

            Console.SetCursorPosition(_left, _top + index);
            Console.ForegroundColor = selected ? Colors.SelectedForeground : Colors.Text;
            Console.BackgroundColor = selected ? Colors.SelectedBackground : ConsoleColor.Black;
            Console.Write($" {(selected ? "►" : " ")} {_items[index].PadRight(_textWidth)} ");
            Console.ResetColor();
        }
    }
}

