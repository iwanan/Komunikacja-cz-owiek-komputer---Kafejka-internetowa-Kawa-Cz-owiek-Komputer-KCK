using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    // pole tekstowe w jednej linii, opcjonalnie z maskowaniem hasła (*)
    public class TextBox
    {
        private readonly int _left;
        private readonly int _top;
        private readonly int _width;
        private readonly bool _masked;
        private readonly int _maxLength;
        private string _text = "";

        public TextBox(int left, int top, int width, bool masked = false, int maxLength = 30)
        {
            _left = left;
            _top = top;
            _width = width;
            _masked = masked;
            _maxLength = maxLength;
        }

        public string Text
        {
            get => _text;
            set => _text = value ?? "";
        }

        // true, gdy Read() przerwano z powodu zmiany rozmiaru okna
        public bool Resized { get; private set; }

        // edycja pola, zwraca klawisz kończący (Enter, Esc, Tab, strzałka) albo null po zmianie rozmiaru
        public ConsoleKey? Read()
        {
            Resized = false;
            ConsoleHelper.TryShowCursor();
            Draw(true);

            while (true)
            {
                if (!ConsoleHelper.WaitForKey(out ConsoleKeyInfo key))
                {
                    Resized = true;
                    ConsoleHelper.TryHideCursor();
                    return null;
                }

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                    case ConsoleKey.Escape:
                    case ConsoleKey.Tab:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        ConsoleHelper.TryHideCursor();
                        Draw(false);
                        return key.Key;

                    case ConsoleKey.Backspace:
                        if (_text.Length > 0)
                        {
                            _text = _text[..^1];
                            Draw(true);
                        }
                        break;

                    default:
                        if (!char.IsControl(key.KeyChar) && _text.Length < _maxLength)
                        {
                            _text += key.KeyChar;
                            Draw(true);
                        }
                        break;
                }
            }
        }

        // rysuje tylko linię pola, active = podświetlone i z kursorem
        public void Draw(bool active)
        {
            string shown = _masked ? new string('*', _text.Length) : _text;

            // zostawiamy miejsce na kursor, długi tekst pokazuje końcówkę
            int visible = _width - 1;
            if (shown.Length > visible) shown = shown[^visible..];

            Console.SetCursorPosition(_left, _top);
            Console.ForegroundColor = active ? Colors.SelectedForeground : Colors.Text;
            Console.BackgroundColor = active ? Colors.SelectedBackground : ConsoleColor.DarkGray;
            Console.Write(shown.PadRight(_width));
            Console.ResetColor();

            if (active)
                Console.SetCursorPosition(_left + shown.Length, _top);
        }
    }
}