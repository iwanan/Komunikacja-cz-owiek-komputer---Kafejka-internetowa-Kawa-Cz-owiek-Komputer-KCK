namespace KawaCzlowiekKomputer.Views
{
    public enum EmployeeMenuChoice
    {
        Stations,
        Reservations,
        CafeMenu,
        Prices,
        Subscriptions,
        Employees,
        Logout
    }

    // menu zalogowanego pracownika
    public interface IViewEmployeeMenu
    {
        EmployeeMenuChoice Show(string userName);
    }
}