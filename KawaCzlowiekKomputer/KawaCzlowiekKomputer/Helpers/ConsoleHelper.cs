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

            try { Console.Clear(); }
            catch (IOException) { }

            _drawnWidth = WindowWidth;
            _drawnHeight = WindowHeight;
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