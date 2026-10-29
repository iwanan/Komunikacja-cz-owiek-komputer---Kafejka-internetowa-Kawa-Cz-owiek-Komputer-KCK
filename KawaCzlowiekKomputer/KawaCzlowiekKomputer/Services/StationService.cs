using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class StationService
    {
        private static readonly string[] Types = { "Standard", "Gaming", "VIP" };

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

        // stanowiska z ręcznym stanem zajętości, bez rezerwacji, do edycji przez pracownika
        public List<Station> GetForEdit() => _stations.GetAll().OrderBy(s => s.Id).ToList();

        public (bool Success, string Message) Add(string name, string type, string cpu, string gpu, string ram)
        {
            string? error = Validate(0, name, type, cpu, gpu, ram);
            if (error is not null) return (false, error);

            _stations.Add(new Station
            {
                Name = name.Trim(),
                Type = CanonicalType(type),
                Cpu = cpu.Trim(),
                Gpu = gpu.Trim(),
                Ram = ram.Trim()
            });
            return (true, "Stanowisko zostało dodane.");
        }

        public (bool Success, string Message) Update(int id, string name, string type, string cpu, string gpu, string ram)
        {
            Station? station = _stations.GetById(id);
            if (station is null) return (false, "Nie znaleziono stanowiska.");

            string? error = Validate(id, name, type, cpu, gpu, ram);
            if (error is not null) return (false, error);

            station.Name = name.Trim();
            station.Type = CanonicalType(type);
            station.Cpu = cpu.Trim();
            station.Gpu = gpu.Trim();
            station.Ram = ram.Trim();
            _stations.Update(station);
            return (true, "Zmiany zostały zapisane.");
        }

        // przełącza ręczny stan zajętości (klient przy komputerze bez rezerwacji)
        public void ToggleOccupied(int id)
        {
            Station? station = _stations.GetById(id);
            if (station is null) return;

            station.IsOccupied = !station.IsOccupied;
            _stations.Update(station);
        }

        // stanowisko z nadchodzącymi rezerwacjami nie może zostać usunięte
        public (bool Success, string Message) Delete(int id)
        {
            if (_reservations.HasUpcoming(id))
                return (false, "Stanowisko ma nadchodzące rezerwacje. Najpierw je anuluj.");

            _stations.Delete(id);
            return (true, "Stanowisko zostało usunięte.");
        }

        // zwraca opis błędu albo null, id = 0 dla nowego stanowiska
        private string? Validate(int id, string name, string type, string cpu, string gpu, string ram)
        {
            if (name.Trim().Length == 0 || cpu.Trim().Length == 0 ||
                gpu.Trim().Length == 0 || ram.Trim().Length == 0)
                return "Wypełnij wszystkie pola.";

            if (!Types.Any(t => t.Equals(type.Trim(), StringComparison.OrdinalIgnoreCase)))
                return "Typ musi być: Standard, Gaming lub VIP.";

            bool nameTaken = _stations.GetAll().Any(s =>
                s.Id != id && s.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
            return nameTaken ? "Stanowisko o tej nazwie już istnieje." : null;
        }

        // typ w zapisie z listy, niezależnie od wielkości liter wpisanych przez pracownika
        private static string CanonicalType(string type) =>
            Types.First(t => t.Equals(type.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}