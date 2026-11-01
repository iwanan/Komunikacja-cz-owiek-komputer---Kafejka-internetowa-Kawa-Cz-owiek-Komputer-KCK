using KawaCzlowiekKomputer.Models;

namespace KawaCzlowiekKomputer.Views
{
    public interface IViewEditEmployees
    {
        // lista pracowników z pozycją "nowy" na końcu, zwraca indeks (Count = nowy) albo null po Esc
        // currentId to Id zalogowanego pracownika, oznaczany na liście
        int? PickEmployee(List<Employee> employees, int currentId);

        // formularz nowego pracownika, wpisane login i e-mail wracają po błędzie, null po Esc
        (string Login, string Email, string Password, string Confirm)? Ask(string? error, string login, string email);

        // komunikat na całym ekranie
        void ShowMessage(string text, bool success);
    }
}
