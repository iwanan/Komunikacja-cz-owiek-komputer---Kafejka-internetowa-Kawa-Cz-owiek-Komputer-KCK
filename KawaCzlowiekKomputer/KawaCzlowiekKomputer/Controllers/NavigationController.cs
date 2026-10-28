using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class NavigationController
    {
        private readonly IViewMainMenu _mainMenu;
        private readonly IViewClientMenu _clientMenu;
        private readonly ClientController _client;
        private readonly AuthController _auth;

        public NavigationController(IViewMainMenu mainMenu, IViewClientMenu clientMenu,
                                    ClientController client, AuthController auth)
        {
            _mainMenu = mainMenu;
            _clientMenu = clientMenu;
            _client = client;
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
                        _auth.LoginEmployee();
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

                    case ClientMenuChoice.Rules:
                        _client.ShowRules();
                        break;

                    default:
                        // pozostałe funkcje dojdą w kolejnych krokach
                        _clientMenu.ShowMessage("Ta funkcja będzie dostępna wkrótce.");
                        break;
                }
            }
        }
    }
}