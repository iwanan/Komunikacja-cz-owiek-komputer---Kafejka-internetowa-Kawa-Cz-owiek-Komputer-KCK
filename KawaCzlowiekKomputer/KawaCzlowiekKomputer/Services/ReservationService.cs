using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class ReservationService
    {
        public const int OpenHour = 10;   // kafejka otwarta od 10:00
        public const int CloseHour = 22;  // do 22:00
        public const int MaxHours = 4;    // najdłuższa jedna rezerwacja
        public const int DaysAhead = 7;   // na ile dni naprzód można rezerwować

        private readonly IRepo<Reservation> _reservations;

        public ReservationService(IRepo<Reservation> reservations) => _reservations = reservations;

        // dni, na które można rezerwować, od dziś
        public List<DateTime> GetDays() =>
            Enumerable.Range(0, DaysAhead).Select(i => DateTime.Today.AddDays(i)).ToList();

        // godziny rozpoczęcia z informacją, czy są wolne
        public List<TimeSlot> GetSlots(int stationId, DateTime day)
        {
            var slots = new List<TimeSlot>();
            for (int h = OpenHour; h < CloseHour; h++)
                slots.Add(new TimeSlot { Hour = h, IsFree = IsFree(stationId, day.Date.AddHours(h)) });
            return slots;
        }

        // ile kolejnych wolnych godzin od startu, maksymalnie MaxHours i do zamknięcia
        public int GetMaxHours(int stationId, DateTime start)
        {
            int hours = 0;
            while (hours < MaxHours
                   && start.AddHours(hours + 1) <= start.Date.AddHours(CloseHour)
                   && IsFree(stationId, start.AddHours(hours)))
                hours++;
            return hours;
        }

        // zapisuje rezerwację po ponownym sprawdzeniu kolizji
        public (bool Success, string Message) Create(int clientId, int stationId, DateTime start, int hours)
        {
            if (hours < 1 || hours > GetMaxHours(stationId, start))
                return (false, "To stanowisko jest już zajęte w wybranym czasie.");

            _reservations.Add(new Reservation { ClientId = clientId, StationId = stationId, Start = start, Hours = hours });
            return (true, "Rezerwacja została zapisana.");
        }

        // nadchodzące i trwające rezerwacje klienta
        public List<Reservation> GetForClient(int clientId) =>
            _reservations.GetAll()
                .Where(r => r.ClientId == clientId && r.Start.AddHours(r.Hours) > DateTime.Now)
                .OrderBy(r => r.Start)
                .ToList();
                // wszystkie rezerwacje klienta, także minione, od najnowszych
                
        public List<Reservation> GetHistory(int clientId) =>
            _reservations.GetAll()
                .Where(r => r.ClientId == clientId)
                .OrderByDescending(r => r.Start)
                .ToList();

        // klient może anulować tylko własną rezerwację
        public bool Cancel(int reservationId, int clientId)
        {
            Reservation? r = _reservations.GetById(reservationId);
            if (r is null || r.ClientId != clientId) return false;

            _reservations.Delete(reservationId);
            return true;
        }

        // czy stanowisko jest zarezerwowane w tej chwili
        public bool IsReservedNow(int stationId)
        {
            DateTime now = DateTime.Now;
            return _reservations.GetAll().Any(r =>
                r.StationId == stationId && r.Start <= now && now < r.Start.AddHours(r.Hours));
        }

        // godzina jest wolna, gdy nie minęła i nie nakłada się na inną rezerwację
        private bool IsFree(int stationId, DateTime start)
        {
            if (start < DateTime.Now) return false;

            DateTime end = start.AddHours(1);
            return !_reservations.GetAll().Any(r =>
                r.StationId == stationId && r.Start < end && start < r.Start.AddHours(r.Hours));
        }
    }
}