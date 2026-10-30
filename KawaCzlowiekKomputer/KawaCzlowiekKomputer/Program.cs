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
            IViewEmployeeMenu employeeMenu = new ScreenEmployeeMenu();
            IViewRules viewRules = new ScreenRules();
            IViewStations viewStations = new ScreenStations();
            IViewReservation viewReservation = new ScreenReservation();
            IViewPrices viewPrices = new ScreenPrices();
            IViewSubs viewSubs = new ScreenSubs();
            IViewGames viewGames = new ScreenGames();
            IViewMenu viewMenu = new ScreenMenu();
            IViewAccount viewAccount = new ScreenAccount();
            IViewLogin viewLogin = new ScreenLogin();
            IViewRegistration viewRegistration = new ScreenRegistration();
            IViewEditStations viewEditStations = new ScreenEditStations();
            IViewEditReservation viewEditReservation = new ScreenEditReservation();
            IViewEditMenu viewEditMenu = new ScreenEditMenu();
            IViewEditPrices viewEditPrices = new ScreenEditPrices();
            IViewEditSubs viewEditSubs = new ScreenEditSubs();

            // dane i logika
            IRepo<Rules> rulesRepo = new JsonRepo<Rules>("Rules.json");
            IRepo<PriceListing> pricesRepo = new JsonRepo<PriceListing>("Prices.json");
            IRepo<Subscription> subsRepo = new JsonRepo<Subscription>("Subscriptions.json");
            IRepo<Game> gamesRepo = new JsonRepo<Game>("Games.json");
            IRepo<MenuItem> menuRepo = new JsonRepo<MenuItem>("Menu.json");
            IRepo<Station> stationsRepo = new JsonRepo<Station>("Stations.json");
            IRepo<Reservation> reservationsRepo = new JsonRepo<Reservation>("Reservations.json");
            IRepo<Purchase> purchasesRepo = new JsonRepo<Purchase>("Purchases.json");
            IRepo<Client> clientsRepo = new JsonRepo<Client>("Users.json");
            IRepo<Employee> employeesRepo = new JsonRepo<Employee>("Employees.json");

            var cafe = new CafeService(rulesRepo, pricesRepo, gamesRepo, menuRepo);
            var subService = new SubService(subsRepo);
            var purchaseService = new PurchaseService(purchasesRepo);
            var reservationService = new ReservationService(reservationsRepo);
            var stationService = new StationService(stationsRepo, reservationService);
            var authService = new AuthService(clientsRepo, employeesRepo);

            var client = new ClientController(cafe, stationService, subService, purchaseService,
                                              viewRules, viewStations, viewPrices, viewSubs, viewGames, viewMenu);
            var reservation = new ReservationController(reservationService, stationService, viewReservation);
            var account = new AccountController(authService, reservationService, stationService,
                                                purchaseService, viewAccount);
            var employee = new EmployeeController(authService, stationService, reservationService,
                                                  viewEditStations, viewEditReservation);
            var catalog = new CatalogController(cafe, subService, viewEditMenu, viewEditPrices, viewEditSubs);
            var auth = new AuthController(authService, viewLogin, viewRegistration);
            var navigation = new NavigationController(mainMenu, clientMenu, employeeMenu, client, reservation,
                                                      account, employee, catalog, auth);
            navigation.Run();
            Console.ResetColor();
            ConsoleHelper.TryClear();
            ConsoleHelper.TryShowCursor();
        }
    }
}