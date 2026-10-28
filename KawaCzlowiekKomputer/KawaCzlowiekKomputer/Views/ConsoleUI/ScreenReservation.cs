using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenReservation : IViewReservation
    {
        public ReservationMenuChoice ShowMenu()
        {
            string[] items = { "Nowa rezerwacja", "Moje rezerwacje", "Wróć" };
            int? result = Picker.FromMenu("Rezerwacje", "Wybierz, co chcesz zrobić", items);

            return result switch
            {
                0 => ReservationMenuChoice.New,
                1 => ReservationMenuChoice.Mine,
                _ => ReservationMenuChoice.Back // Esc albo "Wróć"
            };
        }

        public int? PickReservation(List<Reservation> reservations, Dictionary<int, string> stationNames)
        {
            var rows = reservations.Select(r => new[]
            {
                stationNames.GetValueOrDefault(r.StationId, "?"),
                r.Start.ToString("yyyy-MM-dd"),
                $"{r.Start:HH:mm}-{r.Start.AddHours(r.Hours):HH:mm}",
                $"{r.Hours} h"
            }).ToList();

            string hint = rows.Count == 0 ? "Nie masz żadnych rezerwacji." : "Enter anuluje wybraną rezerwację";
            int? index = Picker.FromTable("Moje rezerwacje", new[] { "Stanowisko", "Data", "Godziny", "Czas" },
                                          rows, hint: hint, enterAction: "Anuluj");

            return index is null ? null : reservations[index.Value].Id;
        }

        public Station? PickStation(List<Station> stations)
        {
            var rows = stations.Select(s => new[] { s.Name, s.Type, s.Gpu, s.Ram }).ToList();
            int? index = Picker.FromTable("Nowa rezerwacja", new[] { "Stanowisko", "Typ", "Karta graficzna", "RAM" },
                                          rows, hint: "Krok 1 z 4: wybierz stanowisko");

            return index is null ? null : stations[index.Value];
        }

        public DateTime? PickDay(List<DateTime> days)
        {
            string[] items = days.Select((d, i) => i == 0
                ? $"Dziś, {d:yyyy-MM-dd}"
                : d.ToString("dddd, yyyy-MM-dd")).ToArray();

            int? index = Picker.FromMenu("Nowa rezerwacja", "Krok 2 z 4: wybierz dzień", items);
            return index is null ? null : days[index.Value];
        }

        public int? PickHour(List<TimeSlot> slots)
        {
            var rows = slots.Select(s => new[]
            {
                $"{s.Hour:00}:00",
                s.IsFree ? "● Wolne" : "● Zajęte"
            }).ToList();

            int? index = Picker.FromTable("Nowa rezerwacja", new[] { "Godzina", "Stan" }, rows,
                rowColor: idx => slots[idx].IsFree ? Colors.Free : Colors.Taken,
                canPick: idx => slots[idx].IsFree,
                hint: "Krok 3 z 4: wybierz godzinę rozpoczęcia");

            return index is null ? null : slots[index.Value].Hour;
        }

        public int? PickDuration(int maxHours)
        {
            string[] items = Enumerable.Range(1, maxHours)
                .Select(h => h == 1 ? "1 godzina" : $"{h} godziny")
                .ToArray();

            int? index = Picker.FromMenu("Nowa rezerwacja", "Krok 4 z 4: na ile godzin", items);
            return index is null ? null : index.Value + 1;
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}