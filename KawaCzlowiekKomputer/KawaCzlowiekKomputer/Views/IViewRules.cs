using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewRules
    {
        // pokazuje regulamin i wraca po naciśnięciu Esc
        void Show(List<Rules> rules);
    }
}
