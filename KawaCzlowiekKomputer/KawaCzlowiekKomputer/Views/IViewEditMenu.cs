using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewEditMenu
    {
        // lista z pozycją "nowa" na końcu, zwraca indeks (Count = nowa) albo null po Esc
        int? PickItem(List<MenuItem> items);

        // menu akcji dla wybranej pozycji, Esc zwraca Back
        EditAction PickAction(string name);

        // formularz pozycji, initial to wartości startowe pól, null po Esc
        (string Name, string Category, string Price, string Restricted)? Ask(string title, string? error, string[]? initial);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}