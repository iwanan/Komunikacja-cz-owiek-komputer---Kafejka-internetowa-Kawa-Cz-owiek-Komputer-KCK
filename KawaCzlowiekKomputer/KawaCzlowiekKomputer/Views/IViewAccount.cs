using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public enum AccountMenuChoice
    {
        Details,
        Reservations,
        Purchases,
        ChangePassword,
        Back
    }

    public interface IViewAccount
    {
        // menu konta, Esc zwraca Back
        AccountMenuChoice ShowMenu(string login);

        // dane konta z wiekiem, wraca po Esc
        void ShowDetails(Client client, int age, bool adult);

        // historia rezerwacji, wraca po Esc
        void ShowReservations(List<Reservation> reservations, Dictionary<int, string> stationNames);

        // historia zakupów z sumą, wraca po Esc
        void ShowPurchases(List<Purchase> purchases, decimal total);

        // formularz zmiany hasła, null po Esc
        (string Old, string New, string Confirm)? AskPassword(string? error);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}