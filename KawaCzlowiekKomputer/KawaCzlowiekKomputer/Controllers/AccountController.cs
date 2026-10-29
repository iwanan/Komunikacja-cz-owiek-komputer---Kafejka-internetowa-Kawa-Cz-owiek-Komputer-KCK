using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Services;
using KawaCzlowiekKomputer.Views;

namespace KawaCzlowiekKomputer.Controllers
{
    public class AccountController
    {
        private readonly AuthService _auth;
        private readonly ReservationService _reservations;
        private readonly StationService _stations;
        private readonly PurchaseService _purchases;
        private readonly IViewAccount _view;

        public AccountController(AuthService auth, ReservationService reservations,
                                 StationService stations, PurchaseService purchases, IViewAccount view)
        {
            _auth = auth;
            _reservations = reservations;
            _stations = stations;
            _purchases = purchases;
            _view = view;
        }

        // menu konta klienta, kończy się wyborem "Wróć"
        public void Run(Client client)
        {
            while (true)
            {
                switch (_view.ShowMenu(client.Login))
                {
                    case AccountMenuChoice.Details:
                        _view.ShowDetails(client, CafeService.GetAge(client.DateOfBirth),
                                          CafeService.IsAdult(client.DateOfBirth));
                        break;

                    case AccountMenuChoice.Reservations:
                        var names = _stations.GetStations().ToDictionary(s => s.Id, s => s.Name);
                        _view.ShowReservations(_reservations.GetHistory(client.Id), names);
                        break;

                    case AccountMenuChoice.Purchases:
                        _view.ShowPurchases(_purchases.GetForClient(client.Id), _purchases.GetTotal(client.Id));
                        break;

                    case AccountMenuChoice.ChangePassword:
                        ChangePassword(client);
                        break;

                    default:
                        return;
                }
            }
        }

        // pokazuje formularz do skutku: poprawna zmiana albo Esc
        private void ChangePassword(Client client)
        {
            string? error = null;

            while (true)
            {
                var data = _view.AskPassword(error);
                if (data is null) return;

                if (data.Value.New != data.Value.Confirm)
                {
                    error = "Nowe hasła nie są takie same.";
                    continue;
                }

                var (success, message) = _auth.ChangePassword(client, data.Value.Old, data.Value.New);
                if (success)
                {
                    _view.ShowMessage(message, true);
                    return;
                }

                error = message;
            }
        }
    }
}