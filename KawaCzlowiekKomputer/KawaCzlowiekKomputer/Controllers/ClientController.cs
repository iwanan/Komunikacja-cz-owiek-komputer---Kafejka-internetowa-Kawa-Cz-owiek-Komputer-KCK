using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class ClientController
    {
        private readonly CafeService _cafe;
        private readonly StationService _stationService;
        private readonly SubService _subService;
        private readonly IViewRules _viewRules;
        private readonly IViewStations _viewStations;
        private readonly IViewPrices _viewPrices;
        private readonly IViewSubs _viewSubs;
        private readonly IViewGames _viewGames;

        public ClientController(CafeService cafe, StationService stationService, SubService subService,
                                IViewRules viewRules, IViewStations viewStations,
                                IViewPrices viewPrices, IViewSubs viewSubs, IViewGames viewGames)
        {
            _cafe = cafe;
            _stationService = stationService;
            _subService = subService;
            _viewRules = viewRules;
            _viewStations = viewStations;
            _viewPrices = viewPrices;
            _viewSubs = viewSubs;
            _viewGames = viewGames;
        }

        // pobiera punkty z serwisu i przekazuje je widokowi
        public void ShowRules() => _viewRules.Show(_cafe.GetRules());

        // pobiera stanowiska z serwisu i przekazuje je widokowi
        public void ShowStations() => _viewStations.Show(_stationService.GetStations());

        public void ShowPrices() => _viewPrices.Show(_cafe.GetPrices());

        public void ShowSubscriptions() => _viewSubs.Show(_subService.GetSubscriptions());

        public void ShowGames() => _viewGames.Show(_cafe.GetGames());
    }
}