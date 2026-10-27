using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Views;
using KawaCzlowiekKomputer.Views.ConsoleUI;

namespace KawaCzlowiekKomputer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // polskie znaki, strzałki, symbole
            ConsoleHelper.TryHideCursor();

            try { Console.Title = "Kawa Człowiek Komputer"; }
            catch (IOException) { } // tytuł okna nie jest krytyczny

            // Jedyne miejsce, które zna konkretną (konsolową) implementację widoku.
            IViewMainMenu mainMenu = new ScreenMainMenu();

            bool running = true;
            while (running)
            {
                switch (mainMenu.Show())
                {
                    case MainMenuChoice.Exit:
                        running = false;
                        break;

                    default:
                        // Na razie żadna funkcja nie jest gotowa.
                        // Docelowo tu przejmie NavigationController.
                        mainMenu.ShowMessage("Ta funkcja będzie dostępna wkrótce.");
                        break;
                }
            }

            Console.ResetColor();
            ConsoleHelper.TryClear();
            ConsoleHelper.TryShowCursor();
        }
    }
}