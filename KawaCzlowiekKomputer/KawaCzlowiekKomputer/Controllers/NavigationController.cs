using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class NavigationController
    {
        private readonly IViewMainMenu _mainMenu;
        private readonly IViewClientMenu _clientMenu;
        private readonly IViewEmployeeMenu _employeeMenu;
        private readonly ClientController _client;
        private readonly ReservationController _reservation;
        private readonly AccountController _account;
        private readonly EmployeeController _employee;
        private readonly AuthController _auth;

        public NavigationController(IViewMainMenu mainMenu, IViewClientMenu clientMenu,
                                    IViewEmployeeMenu employeeMenu,
                                    ClientController client, ReservationController reservation,
                                    AccountController account, EmployeeController employee,
                                    AuthController auth)
        {
            _mainMenu = mainMenu;
            _clientMenu = clientMenu;
            _employeeMenu = employeeMenu;
            _client = client;
            _reservation = reservation;
            _account = account;
            _employee = employee;
            _auth = auth;
        }

        // pętla główna: menu, wybór, przejście do ekranu, powrót do menu
        public void Run()
        {
            bool running = true;
            while (running)
            {
                switch (_mainMenu.Show())
                {
                    case MainMenuChoice.Exit:
                        running = false;
                        break;

                    case MainMenuChoice.Rules:
                        _client.ShowRules();
                        break;

                    case MainMenuChoice.Login:
                        if (_auth.LoginClient()) RunClientMenu();
                        break;

                    case MainMenuChoice.Register:
                        _auth.Register();
                        break;

                    case MainMenuChoice.EmployeeLogin:
                        if (_auth.LoginEmployee()) RunEmployeeMenu();
                        break;
                }
            }
        }

        // menu zalogowanego klienta, kończy się wylogowaniem
        private void RunClientMenu()
        {
            bool inMenu = true;
            while (inMenu)
            {
                switch (_clientMenu.Show(_auth.CurrentClient!.Login))
                {
                    case ClientMenuChoice.Logout:
                        _auth.Logout();
                        inMenu = false;
                        break;

                    case ClientMenuChoice.Stations:
                        _client.ShowStations();
                        break;

                    case ClientMenuChoice.Reservations:
                        _reservation.Run(_auth.CurrentClient!);
                        break;

                    case ClientMenuChoice.Prices:
                        _client.ShowPrices();
                        break;

                    case ClientMenuChoice.Subscriptions:
                        _client.ShowSubscriptions(_auth.CurrentClient!);
                        break;

                    case ClientMenuChoice.Games:
                        _client.ShowGames(_auth.CurrentClient!);
                        break;

                    case ClientMenuChoice.CafeMenu:
                        _client.ShowMenu(_auth.CurrentClient!);
                        break;

                    case ClientMenuChoice.Account:
                        _account.Run(_auth.CurrentClient!);
                        break;

                    case ClientMenuChoice.Rules:
                        _client.ShowRules();
                        break;
                }
            }
        }

        // menu zalogowanego pracownika, kończy się wylogowaniem
        private void RunEmployeeMenu()
        {
            bool inMenu = true;
            while (inMenu)
            {
                switch (_employeeMenu.Show(_auth.CurrentEmployee!.Login))
                {
                    case EmployeeMenuChoice.Logout:
                        _auth.Logout();
                        inMenu = false;
                        break;

                    case EmployeeMenuChoice.Stations:
                        _employee.ManageStations();
                        break;

                    case EmployeeMenuChoice.Reservations:
                        _employee.ManageReservations();
                        break;

                    default:
                        // menu, cennik i karnety dojdą w paczce B
                        _employeeMenu.ShowMessage("Ta funkcja będzie dostępna wkrótce.");
                        break;
                }
            }
        }
    }
}