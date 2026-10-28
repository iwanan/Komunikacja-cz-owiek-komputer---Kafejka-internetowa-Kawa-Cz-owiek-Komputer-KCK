using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewSubs
    {
        // pokazuje karnety i wraca po naciśnięciu Esc
        void Show(List<Subscription> subscriptions);
    }
}