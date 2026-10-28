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

                       // Jedyne miejsce, które zna konkretną (konsolową) implementację widoku.
            IViewMainMenu mainMenu = new ScreenMainMenu();
            IViewClientMenu clientMenu = new ScreenClientMenu();
            IViewRules viewRules = new ScreenRules();
            IViewStations viewStations = new ScreenStations();
            IViewLogin viewLogin = new ScreenLogin();
            IViewRegistration viewRegistration = new ScreenRegistration();

            // dane i logika
            IRepo<Rules> rulesRepo = new JsonRepo<Rules>("Rules.json");
            IRepo<Station> stationsRepo = new JsonRepo<Station>("Stations.json");
            IRepo<Client> clientsRepo = new JsonRepo<Client>("Users.json");
            IRepo<Employee> employeesRepo = new JsonRepo<Employee>("Employees.json");

            var cafe = new CafeService(rulesRepo);
            var stationService = new StationService(stationsRepo);
            var authService = new AuthService(clientsRepo, employeesRepo);

            var client = new ClientController(cafe, stationService, viewRules, viewStations);
            var auth = new AuthController(authService, viewLogin, viewRegistration);
            var navigation = new NavigationController(mainMenu, clientMenu, client, auth);
            navigation.Run();
            Console.ResetColor();
            ConsoleHelper.TryClear();
            ConsoleHelper.TryShowCursor();
        }
    }
}