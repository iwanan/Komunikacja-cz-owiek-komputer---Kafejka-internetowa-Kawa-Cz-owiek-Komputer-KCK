using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewEditSubs
    {
        // lista z pozycją "nowy" na końcu, zwraca indeks (Count = nowy) albo null po Esc
        int? PickSub(List<Subscription> subscriptions);

        // menu akcji dla wybranego karnetu, Esc zwraca Back
        EditAction PickAction(string name);

        // formularz karnetu, initial to wartości startowe pól, null po Esc
        (string Name, string Hours, string ValidDays, string Price)? Ask(string title, string? error, string[]? initial);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}