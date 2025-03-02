using TextRpg.Core.Utilities;
using TextRpg.Game.Managers;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus
{
    public static class ConfirmationMenu
    {
        public static bool Show(string header)
        {
            Logger.LogInfo($"{nameof(ConfirmationMenu)}::{nameof(Show)}", $"Displaying confirmation menu with header: {header}");
            Console.Clear();

            bool confirmed = false;

            MenuItem[,] menuItems = new MenuItem[,]
            {
                { new MenuItem("Yes", () => confirmed = true) },
                { new MenuItem("No", () => confirmed = false) }
            };

            MenuManager menu = new(menuItems);
            menu.ShowMenu(header);

            Logger.LogInfo($"{nameof(ConfirmationMenu)}::{nameof(Show)}", $"Confirmation result: {(confirmed ? "Confirmed" : "Cancelled")}");
            return confirmed;
        }
    }
}
