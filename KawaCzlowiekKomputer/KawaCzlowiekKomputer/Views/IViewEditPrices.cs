using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewEditPrices
    {
        // lista z pozycją "nowa" na końcu, zwraca indeks (Count = nowa) albo null po Esc
        int? PickPrice(List<PriceListing> prices);

        // menu akcji dla wybranej pozycji, Esc zwraca Back
        EditAction PickAction(string name);

        // formularz pozycji cennika, initial to wartości startowe pól, null po Esc
        (string Name, string Price, string Description)? Ask(string title, string? error, string[]? initial);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}