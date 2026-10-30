using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    // edycja menu kawiarni, cennika i karnetów przez pracownika
    public class CatalogController
    {
        private readonly CafeService _cafe;
        private readonly SubService _subService;
        private readonly IViewEditMenu _viewMenu;
        private readonly IViewEditPrices _viewPrices;
        private readonly IViewEditSubs _viewSubs;

        public CatalogController(CafeService cafe, SubService subService,
                                 IViewEditMenu viewMenu, IViewEditPrices viewPrices, IViewEditSubs viewSubs)
        {
            _cafe = cafe;
            _subService = subService;
            _viewMenu = viewMenu;
            _viewPrices = viewPrices;
            _viewSubs = viewSubs;
        }

        // menu kawiarni: lista, Enter otwiera akcje, ostatnia pozycja dodaje nową
        public void ManageMenu()
        {
            while (true)
            {
                List<MenuItem> items = _cafe.GetMenuForEdit();
                int? index = _viewMenu.PickItem(items);
                if (index is null) return;

                if (index.Value == items.Count)
                {
                    MenuForm(null);
                    continue;
                }

                MenuItem item = items[index.Value];
                switch (_viewMenu.PickAction(item.Name))
                {
                    case EditAction.Edit:
                        MenuForm(item);
                        break;

                    case EditAction.Delete:
                        if (!_viewMenu.Confirm($"Usunąć {item.Name}?")) break;
                        _cafe.DeleteMenuItem(item.Id);
                        _viewMenu.ShowMessage("Pozycja została usunięta.", true);
                        break;
                }
            }
        }

        // cennik: ten sam schemat co menu
        public void ManagePrices()
        {
            while (true)
            {
                List<PriceListing> prices = _cafe.GetPrices();
                int? index = _viewPrices.PickPrice(prices);
                if (index is null) return;

                if (index.Value == prices.Count)
                {
                    PriceForm(null);
                    continue;
                }

                PriceListing price = prices[index.Value];
                switch (_viewPrices.PickAction(price.Name))
                {
                    case EditAction.Edit:
                        PriceForm(price);
                        break;

                    case EditAction.Delete:
                        if (!_viewPrices.Confirm($"Usunąć {price.Name}?")) break;
                        _cafe.DeletePrice(price.Id);
                        _viewPrices.ShowMessage("Pozycja cennika została usunięta.", true);
                        break;
                }
            }
        }

        // karnety: ten sam schemat co menu
        public void ManageSubscriptions()
        {
            while (true)
            {
                List<Subscription> subs = _subService.GetSubscriptions();
                int? index = _viewSubs.PickSub(subs);
                if (index is null) return;

                if (index.Value == subs.Count)
                {
                    SubForm(null);
                    continue;
                }

                Subscription sub = subs[index.Value];
                switch (_viewSubs.PickAction(sub.Name))
                {
                    case EditAction.Edit:
                        SubForm(sub);
                        break;

                    case EditAction.Delete:
                        if (!_viewSubs.Confirm($"Usunąć {sub.Name}?")) break;
                        _subService.Delete(sub.Id);
                        _viewSubs.ShowMessage("Karnet został usunięty.", true);
                        break;
                }
            }
        }

        // formularz do skutku: poprawne dane albo Esc, existing = null oznacza nową pozycję
        private void MenuForm(MenuItem? existing)
        {
            string title = existing is null ? "Nowa pozycja menu" : "Edycja pozycji menu";
            string[]? draft = existing is null ? null : new[]
            {
                existing.Name, existing.Category, existing.Price.ToString("0.00"),
                existing.AgeRestricted ? "tak" : "nie"
            };
            string? error = null;

            while (true)
            {
                var data = _viewMenu.Ask(title, error, draft);
                if (data is null) return;

                // wpisane wartości zostają w formularzu po błędzie
                draft = new[] { data.Value.Name, data.Value.Category, data.Value.Price, data.Value.Restricted };

                var (success, message) = existing is null
                    ? _cafe.AddMenuItem(draft[0], draft[1], draft[2], draft[3])
                    : _cafe.UpdateMenuItem(existing.Id, draft[0], draft[1], draft[2], draft[3]);

                if (success)
                {
                    _viewMenu.ShowMessage(message, true);
                    return;
                }

                error = message;
            }
        }

        private void PriceForm(PriceListing? existing)
        {
            string title = existing is null ? "Nowa pozycja cennika" : "Edycja pozycji cennika";
            string[]? draft = existing is null ? null : new[]
            {
                existing.Name, existing.PricePerHour.ToString("0.00"), existing.Description
            };
            string? error = null;

            while (true)
            {
                var data = _viewPrices.Ask(title, error, draft);
                if (data is null) return;

                draft = new[] { data.Value.Name, data.Value.Price, data.Value.Description };

                var (success, message) = existing is null
                    ? _cafe.AddPrice(draft[0], draft[1], draft[2])
                    : _cafe.UpdatePrice(existing.Id, draft[0], draft[1], draft[2]);

                if (success)
                {
                    _viewPrices.ShowMessage(message, true);
                    return;
                }

                error = message;
            }
        }

        private void SubForm(Subscription? existing)
        {
            string title = existing is null ? "Nowy karnet" : "Edycja karnetu";
            string[]? draft = existing is null ? null : new[]
            {
                existing.Name, existing.Hours.ToString(), existing.ValidDays.ToString(),
                existing.Price.ToString("0.00")
            };
            string? error = null;

            while (true)
            {
                var data = _viewSubs.Ask(title, error, draft);
                if (data is null) return;

                draft = new[] { data.Value.Name, data.Value.Hours, data.Value.ValidDays, data.Value.Price };

                var (success, message) = existing is null
                    ? _subService.Add(draft[0], draft[1], draft[2], draft[3])
                    : _subService.Update(existing.Id, draft[0], draft[1], draft[2], draft[3]);

                if (success)
                {
                    _viewSubs.ShowMessage(message, true);
                    return;
                }

                error = message;
            }
        }
    }
}