using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public enum StationActionChoice
    {
        ToggleOccupied,
        Edit,
        Delete,
        Back
    }

    public interface IViewEditStations
    {
        // lista stanowisk z pozycją "nowe" na końcu, zwraca indeks (Count = nowe) albo null po Esc
        int? PickStation(List<Station> stations);

        // menu akcji dla wybranego stanowiska, Esc zwraca Back
        StationActionChoice PickAction(Station station);

        // formularz stanowiska, initial to wartości startowe, null po Esc
        (string Name, string Type, string Cpu, string Gpu, string Ram)? Ask(string title, string? error, Station? initial);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}