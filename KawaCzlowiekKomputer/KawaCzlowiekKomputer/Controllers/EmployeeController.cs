using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class EmployeeController
    {
        private readonly AuthService _auth;
        private readonly StationService _stations;
        private readonly ReservationService _reservations;
        private readonly IViewEditStations _viewStations;
        private readonly IViewEditReservation _viewReservations;
        private readonly IViewEditEmployees _viewEmployees;

        public EmployeeController(AuthService auth, StationService stations, ReservationService reservations,
                                  IViewEditStations viewStations, IViewEditReservation viewReservations,
                                  IViewEditEmployees viewEmployees)
        {
            _viewEmployees = viewEmployees;
            _auth = auth;
            _stations = stations;
            _reservations = reservations;
            _viewStations = viewStations;
            _viewReservations = viewReservations;
        }

        // lista stanowisk, Enter otwiera menu akcji, ostatnia pozycja dodaje nowe
        public void ManageStations()
        {
            while (true)
            {
                List<Station> stations = _stations.GetForEdit();
                int? index = _viewStations.PickStation(stations);
                if (index is null) return;

                if (index.Value == stations.Count)
                {
                    StationForm(null);
                    continue;
                }

                Station station = stations[index.Value];
                switch (_viewStations.PickAction(station))
                {
                    case StationActionChoice.ToggleOccupied:
                        _stations.ToggleOccupied(station.Id);
                        break;

                    case StationActionChoice.Edit:
                        StationForm(station);
                        break;

                    case StationActionChoice.Delete:
                        DeleteStation(station);
                        break;
                }
            }
        }

        // lista pracowników, ostatnia pozycja dodaje nowego, current to zalogowany pracownik
        public void ManageEmployees(Employee current)
        {
            while (true)
            {
                List<Employee> employees = _auth.GetEmployees();
                int? index = _viewEmployees.PickEmployee(employees, current.Id);
                if (index is null) return;

                if (index.Value == employees.Count)
                    EmployeeForm();
            }
        }

        // lista nadchodzących rezerwacji wszystkich klientów, Enter anuluje po potwierdzeniu
        public void ManageReservations()
        {
            while (true)
            {
                List<Reservation> all = _reservations.GetUpcoming();
                var stationNames = _stations.GetForEdit().ToDictionary(s => s.Id, s => s.Name);
                var clientNames = _auth.GetClientNames();

                int? id = _viewReservations.PickReservation(all, stationNames, clientNames);
                if (id is null) return;

                if (!_viewReservations.Confirm("Anulować wybraną rezerwację?")) continue;

                bool ok = _reservations.CancelAsEmployee(id.Value);
                _viewReservations.ShowMessage(
                    ok ? "Rezerwacja została anulowana." : "Nie udało się anulować rezerwacji.", ok);
            }
        }

        // formularz do skutku: poprawne dane albo Esc, existing = null oznacza nowe stanowisko
        private void StationForm(Station? existing)
        {
            string title = existing is null ? "Nowe stanowisko" : "Edycja stanowiska";
            Station? draft = existing;
            string? error = null;

            while (true)
            {
                var data = _viewStations.Ask(title, error, draft);
                if (data is null) return;

                // wpisane wartości zostają w formularzu po błędzie
                draft = new Station
                {
                    Name = data.Value.Name,
                    Type = data.Value.Type,
                    Cpu = data.Value.Cpu,
                    Gpu = data.Value.Gpu,
                    Ram = data.Value.Ram
                };

                var (success, message) = existing is null
                    ? _stations.Add(draft.Name, draft.Type, draft.Cpu, draft.Gpu, draft.Ram)
                    : _stations.Update(existing.Id, draft.Name, draft.Type, draft.Cpu, draft.Gpu, draft.Ram);

                if (success)
                {
                    _viewStations.ShowMessage(message, true);
                    return;
                }

                error = message;
            }
        }

        // formularz do skutku: konto utworzone albo Esc, login i e-mail zostają po błędzie
        private void EmployeeForm()
        {
            string? error = null;
            string login = "";
            string email = "";

            while (true)
            {
                var data = _viewEmployees.Ask(error, login, email);
                if (data is null) return;

                login = data.Value.Login;
                email = data.Value.Email;

                if (data.Value.Password != data.Value.Confirm)
                {
                    error = "Hasła nie są takie same.";
                    continue;
                }

                var (success, message) = _auth.AddEmployee(login, email, data.Value.Password);
                if (success)
                {
                    _viewEmployees.ShowMessage(message, true);
                    return;
                }

                error = message;
            }
        }

        private void DeleteStation(Station station)
        {
            if (!_viewStations.Confirm($"Usunąć stanowisko {station.Name}?")) return;

            var (success, message) = _stations.Delete(station.Id);
            _viewStations.ShowMessage(message, success);
        }
    }
}