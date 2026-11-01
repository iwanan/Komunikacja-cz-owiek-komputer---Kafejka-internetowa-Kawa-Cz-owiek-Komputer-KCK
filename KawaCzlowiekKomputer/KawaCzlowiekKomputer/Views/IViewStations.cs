using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewStations
    {
        // pokazuje listę stanowisk, zwraca wybrane po Enter (do rezerwacji) albo null po Esc
        Station? Show(List<Station> stations);
    }
}