namespace KawaCzlowiekKomputer.Views
{
    public enum EmployeeMenuChoice
    {
        Stations,
        Reservations,
        CafeMenu,
        Prices,
        Subscriptions,
        Logout
    }

    // menu zalogowanego pracownika
    public interface IViewEmployeeMenu
    {
        EmployeeMenuChoice Show(string userName);
    }
}