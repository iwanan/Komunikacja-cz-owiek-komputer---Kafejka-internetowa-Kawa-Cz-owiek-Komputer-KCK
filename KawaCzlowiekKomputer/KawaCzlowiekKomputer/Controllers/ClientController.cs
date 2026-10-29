using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class ClientController
    {
        private readonly CafeService _cafe;
        private readonly StationService _stationService;
        private readonly SubService _subService;
        private readonly PurchaseService _purchases;
        private readonly IViewRules _viewRules;
        private readonly IViewStations _viewStations;
        private readonly IViewPrices _viewPrices;
        private readonly IViewSubs _viewSubs;
        private readonly IViewGames _viewGames;
        private readonly IViewMenu _viewMenu;

        public ClientController(CafeService cafe, StationService stationService, SubService subService,
                                PurchaseService purchases,
                                IViewRules viewRules, IViewStations viewStations,
                                IViewPrices viewPrices, IViewSubs viewSubs, IViewGames viewGames,
                                IViewMenu viewMenu)
        {
            _cafe = cafe;
            _stationService = stationService;
            _subService = subService;
            _purchases = purchases;
            _viewRules = viewRules;
            _viewStations = viewStations;
            _viewPrices = viewPrices;
            _viewSubs = viewSubs;
            _viewGames = viewGames;
            _viewMenu = viewMenu;
        }

        // pobiera punkty z serwisu i przekazuje je widokowi
        public void ShowRules() => _viewRules.Show(_cafe.GetRules());

        // pobiera stanowiska z serwisu i przekazuje je widokowi
        public void ShowStations() => _viewStations.Show(_stationService.GetStations());

        public void ShowPrices() => _viewPrices.Show(_cafe.GetPrices());

        // lista karnetów, Enter kupuje wybrany po potwierdzeniu
        public void ShowSubscriptions(Client client)
        {
            while (true)
            {
                List<Subscription> subs = _subService.GetSubscriptions();
                int? index = _viewSubs.Pick(subs);
                if (index is null) return;

                Subscription sub = subs[index.Value];
                if (!_viewSubs.Confirm($"Kupić {sub.Name} za {sub.Price:0.00} zł?")) continue;

                _purchases.Add(client.Id, sub.Name, "Karnet", sub.Price);
                _viewSubs.ShowMessage($"Kupiono: {sub.Name}.", true);
            }
        }

        // dostępność gier zależy od wieku zalogowanego klienta
        public void ShowGames(Client client) => _viewGames.Show(_cafe.GetGames(client));

        // menu kawiarni, Enter zamawia wybraną pozycję po potwierdzeniu
        public void ShowMenu(Client client)
        {
            while (true)
            {
                List<Offer<MenuItem>> items = _cafe.GetMenu(client);
                int? index = _viewMenu.Pick(items);
                if (index is null) return;

                Offer<MenuItem> offer = items[index.Value];
                if (!offer.IsAllowed) continue; // zabezpieczenie niezależne od widoku

                MenuItem item = offer.Item;
                if (!_viewMenu.Confirm($"Zamówić {item.Name} za {item.Price:0.00} zł?")) continue;

                _purchases.Add(client.Id, item.Name, "Menu", item.Price);
                _viewMenu.ShowMessage($"Zamówiono: {item.Name}.", true);
            }
        }
    }
}