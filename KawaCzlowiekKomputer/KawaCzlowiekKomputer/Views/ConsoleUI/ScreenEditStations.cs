using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenEditStations : IViewEditStations
    {
        public int? PickStation(List<Station> stations)
        {
            var rows = stations
                .Select(s => new[] { s.Name, s.Type, s.IsOccupied ? "● Zajęte" : "● Wolne" })
                .ToList();
            rows.Add(new[] { "+ Nowe stanowisko", "", "" });

            return Picker.FromTable("Edycja stanowisk", new[] { "Stanowisko", "Typ", "Stan ręczny" }, rows,
                                    rowColor: i => i == stations.Count
                                        ? Colors.Accent
                                        : stations[i].IsOccupied ? Colors.Taken : Colors.Free,
                                    hint: "Enter otwiera stanowisko, ostatnia pozycja dodaje nowe");
        }

        public StationActionChoice PickAction(Station station)
        {
            string toggle = station.IsOccupied ? "Oznacz jako wolne" : "Oznacz jako zajęte";
            string[] items = { toggle, "Edytuj dane", "Usuń stanowisko", "Wróć" };
            int? result = Picker.FromMenu(station.Name, "Wybierz, co zrobić", items);

            return result switch
            {
                0 => StationActionChoice.ToggleOccupied,
                1 => StationActionChoice.Edit,
                2 => StationActionChoice.Delete,
                _ => StationActionChoice.Back // Esc albo "Wróć"
            };
        }

        public (string Name, string Type, string Cpu, string Gpu, string Ram)? Ask(
            string title, string? error, Station? initial)
        {
            var form = new Form(
                title,
                new[]
                {
                    ("Nazwa", false),
                    ("Typ (Standard, Gaming, VIP)", false),
                    ("Procesor", false),
                    ("Karta graficzna", false),
                    ("Pamięć RAM", false)
                },
                error,
                initial is null ? null : new[] { initial.Name, initial.Type, initial.Cpu, initial.Gpu, initial.Ram });

            string[]? v = form.Show();
            return v is null ? null : (v[0], v[1], v[2], v[3], v[4]);
        }

        public bool Confirm(string question) =>
            Picker.FromMenu(question, null, new[] { "Tak", "Nie" }) == 0;

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}