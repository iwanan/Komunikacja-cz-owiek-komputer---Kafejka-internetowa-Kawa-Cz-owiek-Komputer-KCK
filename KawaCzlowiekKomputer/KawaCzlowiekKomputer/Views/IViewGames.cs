using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewGames
    {
        // pokazuje listę gier i wraca po naciśnięciu Esc
        void Show(List<Offer<Game>> games);
    }
}