using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewMenu
    {
        // pokazuje menu kawiarni i wraca po naciśnięciu Esc
        void Show(List<Offer<MenuItem>> items);
    }
}