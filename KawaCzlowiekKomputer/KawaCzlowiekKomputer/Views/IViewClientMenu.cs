namespace KawaCzlowiekKomputer.Views
{
    public enum ClientMenuChoice
    {
        Stations,
        Reservations,
        Prices,
        Subscriptions,
        Games,
        CafeMenu,
        Rules,
        Account,
        Logout
    }

    // menu zalogowanego klienta
    public interface IViewClientMenu
    {
        ClientMenuChoice Show(string userName);
    }
}