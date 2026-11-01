using KawaCzlowiekKomputer.Helpers;

namespace KawaCzlowiekKomputer.Views.Components
{
    // Ramka z znaków Unicode. Rysuje tylko krawędzie, środek zostaje nietknięty
    public static class Border
    {
        // Opcjonalny tytuł w górnej krawędzi
        public static void Draw(int left, int top, int width, int height,
                                ConsoleColor color = Colors.Border, string? title = null)
        {
            if (width < 2 || height < 2) return;

            int inner = width - 2;
            string line;
            if (title is null)
                line = new string('─', inner);
            else
            {
                string t = $"─ {title} ";
                if (t.Length > inner) t = t[..inner]; // tytuł nie może wyjść poza ramkę
                line = t.PadRight(inner, '─');
            }

            Console.ForegroundColor = color;

            ConsoleHelper.SetCursor(left, top);
            Console.Write("┌" + line + "┐");

            for (int y = 1; y < height - 1; y++)
            {
                ConsoleHelper.SetCursor(left, top + y);
                Console.Write("│");
                ConsoleHelper.SetCursor(left + width - 1, top + y);
                Console.Write("│");
            }

            ConsoleHelper.SetCursor(left, top + height - 1);
            Console.Write("└" + new string('─', inner) + "┘");

            Console.ResetColor();
        }
    }
}