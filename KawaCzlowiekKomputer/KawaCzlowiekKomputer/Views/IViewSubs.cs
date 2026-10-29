using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewSubs
    {
        // lista karnetów, zwraca indeks wybranego do kupienia albo null po Esc
        int? Pick(List<Subscription> subscriptions);

        // pytanie tak/nie
        bool Confirm(string question);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}