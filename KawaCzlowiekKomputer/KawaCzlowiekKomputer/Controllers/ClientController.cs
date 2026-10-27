using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class ClientController
    {
        private readonly CafeService _cafe;
        private readonly IViewRules _viewRules;

        public ClientController(CafeService cafe, IViewRules viewRules)
        {
            _cafe = cafe;
            _viewRules = viewRules;
        }

        // pobiera punkty z serwisu i przekazuje je widokowi
        public void ShowRules() => _viewRules.Show(_cafe.GetRules());
    }
}