using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewStations
    {
        // pokazuje listę stanowisk i wraca po naciśnięciu Esc
        void Show(List<Station> stations);
    }
}