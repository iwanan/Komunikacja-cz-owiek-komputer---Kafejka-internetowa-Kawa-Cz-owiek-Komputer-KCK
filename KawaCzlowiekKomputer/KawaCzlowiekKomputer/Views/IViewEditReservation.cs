using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public enum ReservationActionChoice
    {
        CheckIn,
        Cancel,
        Back
    }

    public interface IViewEditReservation
    {
        // rezerwacje wszystkich klientów, zwraca indeks wybranej albo null po Esc
        int? PickReservation(List<Reservation> reservations,
                             Dictionary<int, string> stationNames, Dictionary<int, string> clientNames);

        // menu akcji dla wybranej rezerwacji, Esc zwraca Back
        ReservationActionChoice PickAction(string title);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}