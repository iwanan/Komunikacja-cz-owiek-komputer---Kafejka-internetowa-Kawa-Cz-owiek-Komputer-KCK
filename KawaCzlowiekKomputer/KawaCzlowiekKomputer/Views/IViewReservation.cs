using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public enum ReservationMenuChoice
    {
        New,
        Mine,
        Back
    }

    public interface IViewReservation
    {
        // menu rezerwacji, Esc zwraca Back
        ReservationMenuChoice ShowMenu();

        // lista rezerwacji klienta, zwraca Id wybranej do anulowania albo null po Esc
        int? PickReservation(List<Reservation> reservations, Dictionary<int, string> stationNames);

        // wybór stanowiska, null po Esc
        Station? PickStation(List<Station> stations);

        // wybór dnia, null po Esc
        DateTime? PickDay(List<DateTime> days);

        // wybór godziny rozpoczęcia (tylko wolne), zwraca godzinę albo null po Esc
        int? PickHour(List<TimeSlot> slots);

        // wybór liczby godzin od 1 do maxHours, null po Esc
        int? PickDuration(int maxHours);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}