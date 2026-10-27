using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class NavigationController
    {
        private readonly IViewMainMenu _mainMenu;
        private readonly ClientController _client;

        public NavigationController(IViewMainMenu mainMenu, ClientController client)
        {
            _mainMenu = mainMenu;
            _client = client;
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

                    default:
                        // pozostałe funkcje dojdą w kolejnych krokach
                        _mainMenu.ShowMessage("Ta funkcja będzie dostępna wkrótce.");
                        break;
                }
            }
        }
    }
}