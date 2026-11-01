using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Repos;

namespace KawaCzlowiekKomputer.Services
{
    public class ReservationService
    {
        public const int OpenHour = 10;            // kafejka otwarta od 10:00
        public const int CloseHour = 22;           // do 22:00
        public const int MaxHours = 4;             // najdłuższa jedna rezerwacja
        public const int DaysAhead = 7;            // na ile dni naprzód można rezerwować
        public const int GracePeriodMinutes = 15;  // tyle stanowisko czeka na klienta

        private readonly IRepo<Reservation> _reservations;
        private readonly SubService _subs;

        public ReservationService(IRepo<Reservation> reservations, SubService subs)
        {
            _reservations = reservations;
            _subs = subs;
        }

        // wygasła = klient się nie zgłosił w ciągu 15 minut od startu
        public static bool IsExpired(Reservation r) =>
            !r.CheckedIn && DateTime.Now > r.Start.AddMinutes(GracePeriodMinutes);

        // rezerwacja blokuje stanowisko, gdy jeszcze trwa i nie wygasła
        private static bool IsActive(Reservation r) =>
            r.Start.AddHours(r.Hours) > DateTime.Now && !IsExpired(r);

        // opis stanu do tabel
        public static string GetStatus(Reservation r)
        {
            if (r.Start.AddHours(r.Hours) <= DateTime.Now) return r.CheckedIn ? "Zrealizowana" : "Wygasła";
            if (IsExpired(r)) return "Wygasła";
            if (r.CheckedIn) return "Na miejscu";
            if (DateTime.Now >= r.Start)
                return $"Przyjdź do {r.Start.AddMinutes(GracePeriodMinutes):HH:mm}";
            return "Oczekuje";
        }

        // zwraca godziny na karnet za wygasłe rezerwacje, każdą tylko raz
        private void ReleaseExpired()
        {
            foreach (Reservation r in _reservations.GetAll())
            {
                if (!IsExpired(r) || r.ClientSubscriptionId is null) continue;

                _subs.Refund(r.ClientSubscriptionId.Value, r.Hours);
                r.ClientSubscriptionId = null; // znacznik, że zwrot już był
                _reservations.Update(r);
            }
        }

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

        // zapisuje rezerwację po ponownym sprawdzeniu kolizji,
        // z karnetem odlicza godziny dopiero, gdy termin jest na pewno wolny
        public (bool Success, string Message) Create(int clientId, int stationId, DateTime start, int hours,
                                                     int? clientSubscriptionId = null)
        {
            ReleaseExpired(); // saldo karnetu ma być aktualne

            if (hours < 1 || hours > GetMaxHours(stationId, start))
                return (false, "To stanowisko jest już zajęte w wybranym czasie.");

            if (clientSubscriptionId is int subId && !_subs.TryUse(subId, clientId, start, hours))
                return (false, "Karnet nie ma wystarczającej liczby godzin albo wygasa przed tym terminem.");

            _reservations.Add(new Reservation
            {
                ClientId = clientId,
                StationId = stationId,
                Start = start,
                Hours = hours,
                ClientSubscriptionId = clientSubscriptionId
            });

            return (true, clientSubscriptionId is null
                ? "Rezerwacja została zapisana."
                : "Rezerwacja została zapisana, godziny odliczono z karnetu.");
        }

        // czy anulowanie tej rezerwacji zwróci godziny na karnet (tylko jeszcze niezaczęta)
        public bool WillRefund(Reservation r) => r.ClientSubscriptionId is not null && r.Start > DateTime.Now;

        // nadchodzące i trwające rezerwacje klienta, bez wygasłych
        public List<Reservation> GetForClient(int clientId)
        {
            ReleaseExpired();
            return _reservations.GetAll()
                .Where(r => r.ClientId == clientId && IsActive(r))
                .OrderBy(r => r.Start)
                .ToList();
        }

        // wszystkie rezerwacje klienta, także minione i wygasłe, od najnowszych
        public List<Reservation> GetHistory(int clientId)
        {
            ReleaseExpired();
            return _reservations.GetAll()
                .Where(r => r.ClientId == clientId)
                .OrderByDescending(r => r.Start)
                .ToList();
        }

        // klient może anulować tylko własną rezerwację
        public bool Cancel(int reservationId, int clientId)
        {
            Reservation? r = _reservations.GetById(reservationId);
            if (r is null || r.ClientId != clientId) return false;

            Remove(r);
            return true;
        }

        // wszystkie aktywne rezerwacje, od najbliższej
        public List<Reservation> GetUpcoming()
        {
            ReleaseExpired();
            return _reservations.GetAll()
                .Where(IsActive)
                .OrderBy(r => r.Start)
                .ToList();
        }

        // anulowanie przez pracownika, bez sprawdzania właściciela
        public bool CancelAsEmployee(int reservationId)
        {
            Reservation? r = _reservations.GetById(reservationId);
            if (r is null) return false;

            Remove(r);
            return true;
        }

        // pracownik potwierdza, że klient przyszedł, dopóki rezerwacja nie wygasła
        public (bool Success, string Message) CheckIn(int reservationId)
        {
            Reservation? r = _reservations.GetById(reservationId);
            if (r is null) return (false, "Nie znaleziono rezerwacji.");
            if (IsExpired(r)) return (false, "Rezerwacja już wygasła.");
            if (r.Start > DateTime.Now) return (false, "Rezerwacja jeszcze się nie rozpoczęła.");

            r.CheckedIn = true;
            _reservations.Update(r);
            return (true, "Zaznaczono, że klient jest na miejscu.");
        }

        // usuwa rezerwację i zwraca godziny na karnet, jeśli rezerwacja jeszcze się nie zaczęła
        private void Remove(Reservation r)
        {
            if (WillRefund(r))
                _subs.Refund(r.ClientSubscriptionId!.Value, r.Hours);

            _reservations.Delete(r.Id);
        }

        // czy stanowisko ma nadchodzące lub trwające rezerwacje
        public bool HasUpcoming(int stationId) =>
            _reservations.GetAll().Any(r => r.StationId == stationId && IsActive(r));

        // czy stanowisko jest zarezerwowane w tej chwili
        public bool IsReservedNow(int stationId)
        {
            DateTime now = DateTime.Now;
            return _reservations.GetAll().Any(r =>
                r.StationId == stationId && r.Start <= now && now < r.Start.AddHours(r.Hours) && !IsExpired(r));
        }

        // godzina jest wolna, gdy nie minęła i nie nakłada się na aktywną rezerwację
        private bool IsFree(int stationId, DateTime start)
        {
            if (start < DateTime.Now) return false;

            DateTime end = start.AddHours(1);
            return !_reservations.GetAll().Any(r =>
                r.StationId == stationId && !IsExpired(r) && r.Start < end && start < r.Start.AddHours(r.Hours));
        }
    }
}