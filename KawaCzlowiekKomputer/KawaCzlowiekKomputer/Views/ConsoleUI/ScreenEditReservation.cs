using KawaCzlowiekKomputer.Models;
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
                $"{r.Hours} h"
            }).ToList();

            string hint = rows.Count == 0 ? "Brak nadchodzących rezerwacji." : "Enter anuluje wybraną rezerwację";
            int? index = Picker.FromTable("Rezerwacje klientów",
                                          new[] { "Klient", "Stanowisko", "Data", "Godziny", "Czas" },
                                          rows, hint: hint, enterAction: "Anuluj");

            return index is null ? null : reservations[index.Value].Id;
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}