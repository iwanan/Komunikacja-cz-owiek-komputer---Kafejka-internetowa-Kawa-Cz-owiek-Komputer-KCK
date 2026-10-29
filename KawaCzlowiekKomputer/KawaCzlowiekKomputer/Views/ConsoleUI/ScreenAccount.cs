using KawaCzlowiekKomputer.Helpers;
using KawaCzlowiekKomputer.Models;
using KawaCzlowiekKomputer.Views.Components;

namespace KawaCzlowiekKomputer.Views.ConsoleUI
{
    public class ScreenAccount : IViewAccount
    {
        public AccountMenuChoice ShowMenu(string login)
        {
            string[] items = { "Dane konta", "Historia rezerwacji", "Historia zakupów", "Zmiana hasła", "Wróć" };
            int? result = Picker.FromMenu("Moje konto", $"Zalogowano jako: {login}", items);

            return result switch
            {
                0 => AccountMenuChoice.Details,
                1 => AccountMenuChoice.Reservations,
                2 => AccountMenuChoice.Purchases,
                3 => AccountMenuChoice.ChangePassword,
                _ => AccountMenuChoice.Back // Esc albo "Wróć"
            };
        }

        public void ShowDetails(Client client, int age, bool adult)
        {
            string[] labels = { "Login", "E-mail", "Data urodzenia", "Wiek", "Treści 18+" };
            string[] values =
            {
                client.Login,
                client.Email,
                client.DateOfBirth.ToString("yyyy-MM-dd"),
                $"{age} lat",
                adult ? "dostępne" : "niedostępne"
            };
            int labelWidth = labels.Max(l => l.Length);

            // pętla przerysowuje ekran po zmianie rozmiaru okna
            while (true)
            {
                ConsoleHelper.TryClear();

                int contentWidth = labelWidth + 3 + values.Max(v => v.Length);
                int width = Math.Max(contentWidth, 12) + 8;
                int left = Math.Max(0, (ConsoleHelper.WindowWidth - width) / 2);
                int top = 1;

                // statyczne elementy rysujemy raz
                Border.Draw(left, top, width, labels.Length + 4, title: "Dane konta");

                for (int i = 0; i < labels.Length; i++)
                {
                    Console.SetCursorPosition(left + 3, top + 2 + i);
                    Console.ForegroundColor = Colors.Accent;
                    Console.Write(labels[i].PadRight(labelWidth));
                    Console.ForegroundColor = Colors.Text;
                    Console.Write(" : ");

                    // ostatni wiersz kolorujemy wg dostępu do treści 18+
                    Console.ForegroundColor = i == labels.Length - 1
                        ? (adult ? Colors.Success : Colors.Warning)
                        : Colors.Text;
                    Console.Write(values[i]);
                    Console.ResetColor();
                }

                new Legend(("[Esc]", "Wróć")).Draw();

                while (true)
                {
                    if (!ConsoleHelper.WaitForKey(out ConsoleKeyInfo key)) break; // zmiana rozmiaru, przerysuj
                    if (key.Key == ConsoleKey.Escape) return;
                }
            }
        }

        public void ShowReservations(List<Reservation> reservations, Dictionary<int, string> stationNames)
        {
            var rows = reservations.Select(r => new[]
            {
                stationNames.GetValueOrDefault(r.StationId, "?"),
                r.Start.ToString("yyyy-MM-dd"),
                $"{r.Start:HH:mm}-{r.Start.AddHours(r.Hours):HH:mm}",
                $"{r.Hours} h"
            }).ToList();

            string hint = rows.Count == 0
                ? "Nie masz jeszcze żadnych rezerwacji."
                : $"Liczba rezerwacji: {rows.Count}";

            Picker.FromTable("Historia rezerwacji", new[] { "Stanowisko", "Data", "Godziny", "Czas" },
                             rows, hint: hint, readOnly: true);
        }

        public void ShowPurchases(List<Purchase> purchases, decimal total)
        {
            var rows = purchases.Select(p => new[]
            {
                p.Date.ToString("yyyy-MM-dd HH:mm"),
                p.Name,
                p.Category,
                $"{p.Price:0.00} zł"
            }).ToList();

            string hint = rows.Count == 0
                ? "Nie masz jeszcze żadnych zakupów."
                : $"Razem wydano: {total:0.00} zł";

            Picker.FromTable("Historia zakupów", new[] { "Data", "Pozycja", "Rodzaj", "Cena" },
                             rows, hint: hint, readOnly: true);
        }

        public (string Old, string New, string Confirm)? AskPassword(string? error)
        {
            var form = new Form(
                "Zmiana hasła",
                new[]
                {
                    ("Obecne hasło", true),
                    ("Nowe hasło (min. 6 znaków i cyfra)", true),
                    ("Powtórz nowe hasło", true)
                },
                error);

            string[]? v = form.Show();
            return v is null ? null : (v[0], v[1], v[2]);
        }

        public void ShowMessage(string text, bool success) => MessageScreen.Show(text, success);
    }
}