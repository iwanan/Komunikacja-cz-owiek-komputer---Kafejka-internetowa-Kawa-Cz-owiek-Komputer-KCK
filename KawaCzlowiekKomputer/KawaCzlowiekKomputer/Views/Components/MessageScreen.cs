using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    // pełnoekranowy komunikat, zamykany dowolnym klawiszem
    public static class MessageScreen
    {
        public static void Show(string text, bool success)
        {
            // pętla przerysowuje komunikat po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                int row = ConsoleHelper.WindowHeight / 2;
                WriteCentered(text, row, success ? Colors.Success : Colors.Error);
                WriteCentered("Naciśnij dowolny klawisz...", row + 1, Colors.Hint);

                if (ConsoleHelper.WaitForKey(out _)) return;
            }
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