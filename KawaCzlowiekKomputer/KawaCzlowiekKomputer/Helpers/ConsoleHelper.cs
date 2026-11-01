namespace KawaCzlowiekKomputer.Helpers
{
    // Bezpieczne wersje operacji konsoli, które w przekierowanym terminalu
    // (np. Debug Console w VS Code) mogą rzucić IOException
    // Używać w widokach i w Program.cs, nigdy w kontrolerach
    public static class ConsoleHelper
    {
        private static int _drawnWidth;  // rozmiar okna z ostatniego rysowania
        private static int _drawnHeight;
        private const int DefaultWidth = 80;

        // najmniejsze okno, w którym wszystkie ekrany się mieszczą
        private const int MinWidth = 90;
        private const int MinHeight = 26;

        public static void TryHideCursor() => TrySetCursor(false); // ukrywa kursor

        public static void TryShowCursor() => TrySetCursor(true); // pokazuje kursor

        private static void TrySetCursor(bool visible)
        {
            if (Console.IsOutputRedirected) return;

            try { Console.CursorVisible = visible; }
            catch (IOException) { }
            catch (PlatformNotSupportedException) { }
        }

        public static void TryClear() // czysci ekran tylko przy zmianie ekranu
        {
            if (Console.IsOutputRedirected) return;

            WaitForMinSize();

            try { Console.Clear(); }
            catch (IOException) { }

            _drawnWidth = WindowWidth;
            _drawnHeight = WindowHeight;
        }

        // ustawia kursor, pozycja poza oknem jest przycinana zamiast rzucać wyjątek
        public static void SetCursor(int left, int top)
        {
            try
            {
                left = Math.Clamp(left, 0, WindowWidth - 1);
                top = Math.Clamp(top, 0, WindowHeight - 1);
                Console.SetCursorPosition(left, top);
            }
            catch (IOException) { }
            catch (ArgumentOutOfRangeException) { }
        }

        // gdy okno jest za małe, pokazuje komunikat i czeka na powiększenie
        private static void WaitForMinSize()
        {
            if (WindowWidth >= MinWidth && WindowHeight >= MinHeight) return;

            while (WindowWidth < MinWidth || WindowHeight < MinHeight)
            {
                int w = WindowWidth;
                int h = WindowHeight;

                try
                {
                    Console.Clear();

                    string text = $"Powiększ okno do {MinWidth}x{MinHeight}";
                    if (text.Length > w - 1) text = text[..Math.Max(0, w - 1)]; // w wąskim oknie ucinamy tekst
                    SetCursor((w - text.Length) / 2, h / 2);
                    Console.Write(text);
                }
                catch (IOException) { }

                // czekamy na zmianę rozmiaru, nie rysujemy w pętli
                while (WindowWidth == w && WindowHeight == h) Thread.Sleep(100);
            }

            while (Console.KeyAvailable) Console.ReadKey(true); // klawisze wciśnięte w międzyczasie ignorujemy
        }

        // wysokość okna albo 25, gdy nie da się jej odczytać
        public static int WindowHeight
        {
            get
            {
                try { return Console.WindowHeight > 0 ? Console.WindowHeight : 25; }
                catch (IOException) { return 25; }
            }
        }

        // szerokość okna albo 80, gdy nie da się jej odczytać
        public static int WindowWidth
        {
            get
            {
                try { return Console.WindowWidth > 0 ? Console.WindowWidth : DefaultWidth; }
                catch (IOException) { return DefaultWidth; }
            }
        }

        // czeka na klawisz, zwraca false gdy w międzyczasie zmienił się rozmiar okna, cos typu ze rozciagnales okno konsoli albo fullscreen
        public static bool WaitForKey(out ConsoleKeyInfo key)
        {
            while (!Console.KeyAvailable)
            {
                if (WindowWidth != _drawnWidth || WindowHeight != _drawnHeight)
                {
                    key = default;
                    return false;
                }
                Thread.Sleep(50);
            }

            key = Console.ReadKey(true);
            return true;
        }
    }
}