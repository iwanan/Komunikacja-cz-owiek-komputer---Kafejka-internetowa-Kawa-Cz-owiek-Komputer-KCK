using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewMenu
    {
        // menu kawiarni, zwraca indeks wybranej pozycji do zamówienia albo null po Esc
        int? Pick(List<Offer<MenuItem>> items);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}