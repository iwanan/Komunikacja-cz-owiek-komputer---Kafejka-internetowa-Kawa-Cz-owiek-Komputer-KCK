using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewPrices
    {
        // pokazuje cennik i wraca po naciśnięciu Esc
        void Show(List<PriceListing> prices);
    }
}