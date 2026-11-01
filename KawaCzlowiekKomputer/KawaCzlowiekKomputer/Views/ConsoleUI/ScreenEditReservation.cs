using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenEditReservation : IViewEditReservation
    {
        public int? PickReservation(List<Reservation> reservations,
                                    Dictionary<int, string> stationNames, Dictionary<int, string> clientNames)
        {
            var rows = reservations.Select(r => new[]
            {
                clientNames.GetValueOrDefault(r.ClientId, "?"),
                stationNames.GetValueOrDefault(r.StationId, "?"),
                r.Start.ToString("yyyy-MM-dd"),
                $"{r.Start:HH:mm}-{r.Start.AddHours(r.Hours):HH:mm}",
                ReservationService.GetStatus(r)
            }).ToList();

            string hint = rows.Count == 0 ? "Brak nadchodzących rezerwacji." : "Enter otwiera akcje dla rezerwacji";
            return Picker.FromTable("Rezerwacje klientów",
                                    new[] { "Klient", "Stanowisko", "Data", "Godziny", "Stan" },
                                    rows,
                                    rowColor: i => reservations[i].CheckedIn ? Colors.Success : Colors.Text,
                                    hint: hint, enterAction: "Akcje");
        }

        public ReservationActionChoice PickAction(string title)
        {
            int? result = Picker.FromMenu(title, "Wybierz, co zrobić",
                                          new[] { "Klient przyszedł", "Anuluj rezerwację", "Wróć" });

            return result switch
            {
                0 => ReservationActionChoice.CheckIn,
                1 => ReservationActionChoice.Cancel,
                _ => ReservationActionChoice.Back // Esc albo "Wróć"
            };
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}