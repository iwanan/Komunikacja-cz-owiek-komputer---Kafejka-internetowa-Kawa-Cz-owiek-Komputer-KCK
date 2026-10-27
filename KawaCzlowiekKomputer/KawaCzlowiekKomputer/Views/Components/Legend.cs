using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    // Pasek pomocy z dostępnymi klawiszami. Rysowany raz, na dole ekranu.
    public class Legend
    {
        private readonly (string Key, string Action)[] _items;

        // Pary: klawisz i opis, np. ("[Esc]", "Wróć")
        public Legend(params (string Key, string Action)[] items) => _items = items;

        // Gotowa legenda: strzałki, Enter, Esc
        public static Legend Navigation => new(
            ("[↑/↓]", "Wybór"),
            ("[Enter]", "Zatwierdź"),
            ("[Esc]", "Wróć"));

        // Rysuje legendę wyśrodkowaną. Bez parametru: w ostatnim wierszu okna.
        public void Draw(int? row = null)
        {
            int y = row ?? ConsoleHelper.WindowHeight - 1;
            int total = _items.Sum(i => i.Key.Length + 1 + i.Action.Length) + 3 * (_items.Length - 1);
            int left = Math.Max(0, (ConsoleHelper.WindowWidth - total) / 2);

            Console.SetCursorPosition(left, y);
            for (int i = 0; i < _items.Length; i++)
            {
                if (i > 0) Console.Write("   ");

                Console.ForegroundColor = Colors.Accent;
                Console.Write(_items[i].Key);
                Console.ForegroundColor = Colors.Text;
                Console.Write(" " + _items[i].Action);
            }
            Console.ResetColor();
        }
    }
}