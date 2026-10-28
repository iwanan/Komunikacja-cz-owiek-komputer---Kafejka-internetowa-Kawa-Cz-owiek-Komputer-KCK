using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class StationService
    {
        private readonly IRepo<Station> _stations;
        private readonly ReservationService _reservations;

        public StationService(IRepo<Station> stations, ReservationService reservations)
        {
            _stations = stations;
            _reservations = reservations;
        }

        // stanowiska w kolejności Id, zwracamy kopie, żeby nie zmienić danych zapisanych w repozytorium
        public List<Station> GetStations() =>
            _stations.GetAll()
                .OrderBy(s => s.Id)
                .Select(s => new Station
                {
                    Id = s.Id,
                    Name = s.Name,
                    Type = s.Type,
                    Cpu = s.Cpu,
                    Gpu = s.Gpu,
                    Ram = s.Ram,
                    IsOccupied = s.IsOccupied || _reservations.IsReservedNow(s.Id)
                })
                .ToList();
    }
}