using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenPrices : IViewPrices
    {
        public void Show(List<PriceListing> prices)
        {
            var rows = prices
                .Select(p => new[] { p.Name, $"{p.PricePerHour:0.00} zł / h", p.Description })
                .ToList();

            string hint = rows.Count == 0 ? "Brak pozycji w cenniku." : "Cena za godzinę korzystania ze stanowiska";
            Picker.FromTable("Cennik", new[] { "Typ", "Cena", "Opis" }, rows, hint: hint, readOnly: true);
        }
    }
}