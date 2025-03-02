using TextRpg.Core.Services.Data;
using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Game.Managers;
using TextRpg.Core.Utilities;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus.Character
{
    public static class DeleteCharacterMenu
    {
        public static string Show()
        {
            Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(Show)}", "Displaying Character Deletion menu.");
            Console.Clear();

            List<string> characterNames = LoadCharacterNames();
            List<MenuItem> menuItems = [];

            foreach (var characterName in characterNames)
            {
                menuItems.Add(new MenuItem(characterName, null));
            }

            MenuItem[,] menuArray = new MenuItem[menuItems.Count, 1];
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuArray[i, 0] = menuItems[i];
            }

            MenuManager menu = new(menuArray);
            (int selectedRow, int selectedCol) = menu.ShowMenu("== Select a Character to Delete ==");

            if (selectedRow >= 0 && selectedCol == 0)
            {
                string characterName = characterNames[selectedRow];
                Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(Show)}", $"Character '{characterName}' selected for deletion.");

                Console.Clear();
                bool confirmed = ConfirmationMenu.Show($"Are you sure you want to delete {characterName}?");

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
                List<CharacterModel> characters = CharacterDataService.GetData<CharacterModel>(CharacterData.Character);

                foreach (var character in characters)
                {
                    characterNames.Add(character.Name);
                }

                Logger.LogInfo($"{nameof(DeleteCharacterMenu)}::{nameof(LoadCharacterNames)}", $"Loaded {characterNames.Count} character(s).");
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(DeleteCharacterMenu)}::{nameof(LoadCharacterNames)}", "Failed to load character names.", ex);
            }

            return characterNames;
        }
    }
}
