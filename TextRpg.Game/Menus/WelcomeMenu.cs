using TextRpg.Core.Utilities;
using TextRpg.Game.Managers;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus
{
    public class WelcomeMenu
    {
        public static void Show(Action onStartGame, Action onExit)
        {
            Logger.LogInfo($"{nameof(WelcomeMenu)}::{nameof(Show)}", "Displaying Welcome Menu.");
            Console.Clear();

            List<MenuItem> menuItems =
            [
                new MenuItem("Start Game", onStartGame, false),
                new MenuItem("Exit", onExit, false)
            ];

            string header = $"== Welcome to GameName ==\n\n" +
                "This is a fully customizable TextRpg\n" +
                "Use the AdminPanel.exe to customize your data\n" +
                "If this is your first playthrough, You have to copy from Game/BaseData into Game/Data\n" + 
                "To navigate through menus use Arrow keys or WASD and Enter\n";

            MenuManager menu = new(menuItems);
            menu.ShowMenu(header);
        }
    }
}
