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
        private static string SelectedCharacter = "";

        public static string Show(Action onCreateCharacter, Action onDeleteCharacter)
        {
            Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "Displaying Character Selection menu.");
            Console.Clear();

            Dictionary<string, string> characterMenuData = LoadCharacterMenuData();
            List<MenuItem> menuItems = [];

            foreach (var characterData in characterMenuData)
            {
                menuItems.Add(new MenuItem(characterData.Key, () => { SelectedCharacter = characterData.Key; }, false, characterData.Value));
            }

            if(characterMenuData.Count > 0)
            {
                menuItems.Add(new MenuItem("", null, false));
            }

            if (characterMenuData.Count < MaxCharacters)
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "Adding 'Create New Character' option.");
                menuItems.Add(new MenuItem("Create New Character", onCreateCharacter, false));
            }

            if (characterMenuData.Count > 0)
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "Adding 'Delete Character' option.");
                menuItems.Add(new MenuItem("Delete Character", onDeleteCharacter, false));
            }

            MenuManager menu = new(menuItems);
            menu.ShowMenu("== Character Selection ==" +
                $"\nCharacters {characterMenuData.Count} / {MaxCharacters}");

            if (!string.IsNullOrEmpty(SelectedCharacter))
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", $"Character '{characterMenuData[SelectedCharacter]}' selected.");
                return characterMenuData[SelectedCharacter];
            }

            Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(Show)}", "No character selected, returning to previous menu.");
            return "";
        }

        private static Dictionary<string, string> LoadCharacterMenuData()
        {
            Dictionary<string, string> characterMenuData = [];

            try
            {
                Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(LoadCharacterMenuData)}", "Loading character names.");
                var characters = CharacterDataService.GetLoadedCharacters();

                foreach (var character in characters)
                {
                    if (characterMenuData.Count < MaxCharacters)
                    {
                        characterMenuData[character.Key] = $" Lvl. {character.Value.Level} | {character.Value.Race} | {character.Value.CharacterClass}";
                    } 
                    else
                    {
                        break;
                    }
                }
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(CharacterMenu)}::{nameof(LoadCharacterMenuData)}", "Failed to load character names.", ex);
            }

            Logger.LogInfo($"{nameof(CharacterMenu)}::{nameof(LoadCharacterMenuData)}", $"Loaded {characterMenuData.Count} character(s).");
            return characterMenuData;
        }
    }
}