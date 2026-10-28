using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class ReservationController
    {
        private readonly ReservationService _reservations;
        private readonly StationService _stations;
        private readonly IViewReservation _view;

        public ReservationController(ReservationService reservations, StationService stations, IViewReservation view)
        {
            _reservations = reservations;
            _stations = stations;
            _view = view;
        }

        // menu rezerwacji klienta, kończy się wyborem "Wróć"
        public void Run(Client client)
        {
            while (true)
            {
                switch (_view.ShowMenu())
                {
                    case ReservationMenuChoice.New:
                        CreateReservation(client);
                        break;

                    case ReservationMenuChoice.Mine:
                        ShowMine(client);
                        break;

                    default:
                        return;
                }
            }
        }

        // cztery kroki: stanowisko, dzień, godzina, czas trwania; Esc w dowolnym kroku wraca do menu
        private void CreateReservation(Client client)
        {
            Station? station = _view.PickStation(_stations.GetStations());
            if (station is null) return;

            DateTime? day = _view.PickDay(_reservations.GetDays());
            if (day is null) return;

            int? hour = _view.PickHour(_reservations.GetSlots(station.Id, day.Value));
            if (hour is null) return;

            DateTime start = day.Value.Date.AddHours(hour.Value);
            int max = _reservations.GetMaxHours(station.Id, start);
            if (max < 1)
            {
                _view.ShowMessage("Ta godzina została w międzyczasie zajęta.", false);
                return;
            }

            int? hours = _view.PickDuration(max);
            if (hours is null) return;

            var (success, message) = _reservations.Create(client.Id, station.Id, start, hours.Value);
            _view.ShowMessage(success
                ? $"{message} {station.Name}, {start:yyyy-MM-dd HH:mm}, {hours} h."
                : message, success);
        }

        // lista rezerwacji klienta, Enter anuluje wybraną po potwierdzeniu
        private void ShowMine(Client client)
        {
            while (true)
            {
                List<Reservation> mine = _reservations.GetForClient(client.Id);
                var names = _stations.GetStations().ToDictionary(s => s.Id, s => s.Name);

                int? id = _view.PickReservation(mine, names);
                if (id is null) return;

                if (_view.Confirm("Anulować wybraną rezerwację?"))
                {
                    bool ok = _reservations.Cancel(id.Value, client.Id);
                    _view.ShowMessage(ok ? "Rezerwacja została anulowana." : "Nie udało się anulować rezerwacji.", ok);
                }
            }
        }
    }
}