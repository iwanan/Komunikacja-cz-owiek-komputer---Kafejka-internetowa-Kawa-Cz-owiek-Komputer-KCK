using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaCzlowiekKomputer.Views
{
    public enum MainMenuChoice
    {
        Login,
        Register,
        EmployeeLogin,
        Rules,
        Exit
    }

    /// <summary>
    /// Kontrakt ekranu startowego. Kontroler zna tylko ten interfejs,
    /// więc konsolę można później podmienić na GUI.
    /// </summary>
    public interface IViewMainMenu
    {
        MainMenuChoice Show();
        void ShowMessage(string text);
    }
}

