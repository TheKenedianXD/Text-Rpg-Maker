using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Services.Data;
using TextRpg.Core.Models.Enums;
using TextRpg.Game.Managers;
using TextRpg.Core.Utilities;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus.Character
{
    public class CharacterMenu
    {
        private const int MaxCharacters = 10;

        public static string Show(Action onCreateCharacter, Action onDeleteCharacter)
        {
            Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "Displaying Character Selection menu.");
            Console.Clear();

            List<string> characterNames = LoadCharacterNames();
            List<MenuItem> menuItems = [];

            foreach (var characterName in characterNames)
            {
                menuItems.Add(new MenuItem(characterName, null));
            }

            if (characterNames.Count < MaxCharacters)
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "Adding 'Create New Character' option.");
                menuItems.Add(new MenuItem("Create New Character", onCreateCharacter));
            }

            if (characterNames.Count > 0)
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "Adding 'Delete Character' option.");
                menuItems.Add(new MenuItem("Delete Character", onDeleteCharacter));
            }

            MenuItem[,] menuArray = new MenuItem[menuItems.Count, 1];
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuArray[i, 0] = menuItems[i];
            }

            MenuManager menu = new(menuArray);
            (int selectedRow, int selectedCol) = menu.ShowMenu("== Character Selection ==");

            if (selectedRow >= 0 && selectedRow < characterNames.Count && selectedCol == 0)
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", $"Character '{characterNames[selectedRow]}' selected.");
                return characterNames[selectedRow];
            }

            Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "No character selected, returning to previous menu.");
            return "";
        }

        private static List<string> LoadCharacterNames()
        {
            List<string> characterNames = [];

            try
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(LoadCharacterNames)}", "Loading character names.");
                List<CharacterModel> characters = CharacterDataService.GetData<CharacterModel>(CharacterData.Character);

                foreach (var character in characters)
                {
                    if (characterNames.Count < MaxCharacters)
                    {
                        characterNames.Add(character.Name);
                    } else
                    {
                        break;
                    }
                }
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(CharacterMenu)}::{nameof(LoadCharacterNames)}", "Failed to load character names.", ex);
            }

            Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(LoadCharacterNames)}", $"Loaded {characterNames.Count} character(s).");
            return characterNames;
        }
    }
}