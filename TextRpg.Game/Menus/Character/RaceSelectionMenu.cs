using TextRpg.Core.Services.Data;
using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Game.Managers;
using TextRpg.Game.Utilities;
using TextRpg.Core.Utilities;
using TextRpg.Game.Menus.Components;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus.Character
{
    public static class RaceSelectionMenu
    {
        private static string SelectedRace = "";

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
                DataMissingInfo.ConsiderAddingData("Races");
                return "";
            }

            List<MenuItem> menuItems = [];
            foreach (var race in races)
            {
                menuItems.Add(new MenuItem(race.Name, () =>
                {
                    if (ShowRaceDetails(race))
                    {
                        SelectedRace = race.Name;
                    } 
                    else
                    {
                        SelectedRace = "";
                    }
                }, false));
            }

            MenuManager menu = new(menuItems);
            menu.ShowMenu("== Select Race ==");

            if (!string.IsNullOrEmpty(SelectedRace))
            {
                Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", $"Race '{SelectedRace}' selected.");
                return SelectedRace;
            }

            Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(Show)}", "No race selected, returning to previous menu.");
            return "";
        }

        private static bool ShowRaceDetails(RaceModel race)
        {
            Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(ShowRaceDetails)}", $"Displaying details for race '{race.Name}'.");

            Console.Clear();

            List<MenuItem> menuItems = [];
            ConfirmationComponent confirmationMenu = new();
            confirmationMenu.AddToMenu(menuItems);

            MenuManager menu = new(menuItems);
            int selectedIndex = menu.ShowMenu(
                $"{race.Name}\n\n"
                + $"{race.Description}");

            bool confirmed = ConfirmationComponent.HandleSelection(selectedIndex, menuItems);

            Logger.LogInfo($"{nameof(RaceSelectionMenu)}::{nameof(ShowRaceDetails)}",
                $"Race selection confirmation result: {(confirmed ? "Confirmed" : "Cancelled")}");

            return confirmed;
        }
    }
}
