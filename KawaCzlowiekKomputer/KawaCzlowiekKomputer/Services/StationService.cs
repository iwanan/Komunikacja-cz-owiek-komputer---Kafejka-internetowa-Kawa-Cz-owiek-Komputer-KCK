using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class StationService
    {
        private readonly IRepo<Station> _stations;

        public StationService(IRepo<Station> stations) => _stations = stations;

        // stanowiska w kolejności Id
        public List<Station> GetStations() => _stations.GetAll().OrderBy(s => s.Id).ToList();
    }
}