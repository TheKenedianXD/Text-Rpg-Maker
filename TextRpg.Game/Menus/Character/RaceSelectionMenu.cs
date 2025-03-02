using TextRpg.Core.Services.Data;
using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Game.Managers;
using TextRpg.Game.Utilities;
using TextRpg.Core.Utilities;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus.Character
{
    public static class RaceSelectionMenu
    {
        public static string Show()
        {
            Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", "Displaying Race Selection menu.");
            Console.Clear();

            List<RaceModel> races = [];
            try
            {
                races = GameDataService.GetData<RaceModel>(GameData.Races);
                Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", $"Loaded {races.Count} race(s).");
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", "Failed to load race data.", ex);
                return "";
            }

            if (races.Count == 0)
            {
                Logger.LogWarning($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", "No races available for selection.");
                return "";
            }

            List<MenuItem> menuItems = [];
            foreach (var race in races)
            {
                menuItems.Add(new MenuItem(race.Name, () => ShowRaceDetails(race)));
            }

            MenuItem[,] menuArray = new MenuItem[menuItems.Count, 1];
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuArray[i, 0] = menuItems[i];
            }

            MenuManager menu = new(menuArray);
            (int selectedRow, int selectedCol) = menu.ShowMenu("== Select Race ==");

            if (selectedRow >= 0 && selectedCol == 0)
            {
                Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", $"Race '{races[selectedRow].Name}' selected.");
                return races[selectedRow].Name;
            }

            Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", "No race selected, returning to previous menu.");
            return "";
        }

        private static void ShowRaceDetails(RaceModel race)
        {
            Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(ShowRaceDetails)}", $"Displaying details for race '{race.Name}'.");

            Console.Clear();
            GameWriter.CenterText($"Race: {race.Name}");
            Console.WriteLine();
            GameWriter.CenterText(race.Description);
            GameWriter.CenterText("\nPress any key to go back...");
            Console.ReadKey(true);
        }
    }
}
