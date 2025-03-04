using TextRpg.Core.Services.Data;
using TextRpg.Core.Models.Data.Character;
using TextRpg.Game.Managers;
using TextRpg.Core.Utilities;
using TextRpg.Game.Menus.Components;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus.Character
{
    public static class DeleteCharacterMenu
    {
        public static string Show()
        {
            Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(Show)}", "Displaying Character Deletion menu.");
            bool cancelled = false;
            Console.Clear();

            List<string> characterNames = LoadCharacterNames();
            List<MenuItem> menuItems = [];

            foreach (var characterName in characterNames)
            {
                menuItems.Add(new MenuItem(characterName, () => { }, true));
            }

            if(characterNames.Count > 0) 
            {
                menuItems.Add(new MenuItem("", null, false));
                menuItems.Add(new MenuItem("Cancel", () =>
                {
                    cancelled = true;
                }, false));
            }

            MenuManager menu = new(menuItems);
            int selectedIndex = menu.ShowMenu("== Select a Character to Delete ==");

            if (cancelled) return "";

            if (selectedIndex > -1 && selectedIndex < characterNames.Count)
            {
                string characterName = characterNames[selectedIndex];
                Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(Show)}", $"Character '{characterName}' selected for deletion.");

                bool confirmed = ShowDeletionConfirmation(characterName);

                if (confirmed)
                {
                    Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(Show)}", $"Character '{characterName}' confirmed for deletion.");
                    return characterName;
                }

                Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(Show)}", $"Character deletion cancelled for '{characterName}'.");
            }

            Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(Show)}", "No character selected, returning to previous menu.");
            return "";
        }

        private static List<string> LoadCharacterNames()
        {
            List<string> characterNames = [];
            try
            {
                Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(LoadCharacterNames)}", "Loading character names.");
                Dictionary<string, CharacterModel> characters = CharacterDataService.GetLoadedCharacters();

                foreach (var character in characters.Keys)
                {
                    characterNames.Add(character);
                }

                Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(LoadCharacterNames)}", $"Loaded {characterNames.Count} character(s).");
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(DeleteCharacterMenu)}::{nameof(LoadCharacterNames)}", "Failed to load character names.", ex);
            }

            return characterNames;
        }

        private static bool ShowDeletionConfirmation(string characterName)
        {
            Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(ShowDeletionConfirmation)}", "Asking to confirm character deletion.");

            List<MenuItem> menuItems = [];
            ConfirmationComponent confirmationMenu = new();
            confirmationMenu.AddToMenu(menuItems);

            MenuManager menu = new(menuItems);
            var selectedIndex = menu.ShowMenu($"Are you sure you want to delete {characterName}?");

            bool confirmed = ConfirmationComponent.HandleSelection(selectedIndex, menuItems);

            return confirmed;
        }
    }
}
