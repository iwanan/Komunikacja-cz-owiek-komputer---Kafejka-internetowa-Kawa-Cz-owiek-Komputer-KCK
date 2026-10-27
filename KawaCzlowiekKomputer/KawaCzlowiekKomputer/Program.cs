using KawaCzlowiekKomputer.Controllers;
using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;
using KawaCzlowiekKomputer.Views.ConsoleUI;

namespace KawaCzlowiekKomputer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // polskie znaki, strzałki, symbole

            // menu strzałkami wymaga prawdziwego terminala
            if (Console.IsInputRedirected || Console.IsOutputRedirected)
            {
                Console.WriteLine("Uruchom aplikację w prawdziwym terminalu (nie w Debug Console).");
                return;
            }

            ConsoleHelper.TryHideCursor();

            try { Console.Title = "Kawa Człowiek Komputer"; }
            catch (IOException) { } // tytuł okna nie jest krytyczny

            // Jedyne miejsce, które zna konkretną (konsolową) implementację widoku.
            IViewMainMenu mainMenu = new ScreenMainMenu();
            IViewRules viewRules = new ScreenRules();

            // dane i logika
            IRepo<Rules> rulesRepo = new JsonRepo<Rules>("Rules.json");
            var cafe = new CafeService(rulesRepo);

            var client = new ClientController(cafe, viewRules);
            var navigation = new NavigationController(mainMenu, client);
            navigation.Run();

            Console.ResetColor();
            ConsoleHelper.TryClear();
            ConsoleHelper.TryShowCursor();
        }
    }
}