using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class ClientController
    {
        private readonly CafeService _cafe;
        private readonly StationService _stationService;
        private readonly IViewRules _viewRules;
        private readonly IViewStations _viewStations;

        public ClientController(CafeService cafe, StationService stationService,
                                IViewRules viewRules, IViewStations viewStations)
        {
            _cafe = cafe;
            _stationService = stationService;
            _viewRules = viewRules;
            _viewStations = viewStations;
        }

        // pobiera punkty z serwisu i przekazuje je widokowi
        public void ShowRules() => _viewRules.Show(_cafe.GetRules());

        // pobiera stanowiska z serwisu i przekazuje je widokowi
        public void ShowStations() => _viewStations.Show(_stationService.GetStations());
    }
}