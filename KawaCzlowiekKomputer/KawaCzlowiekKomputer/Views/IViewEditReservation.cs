using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewEditReservation
    {
        // rezerwacje wszystkich klientów, zwraca Id wybranej do anulowania albo null po Esc
        int? PickReservation(List<Reservation> reservations,
                             Dictionary<int, string> stationNames, Dictionary<int, string> clientNames);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}