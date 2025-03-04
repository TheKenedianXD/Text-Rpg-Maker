using TextRpg.Core.Services.Data;
using TextRpg.Game.Menus.Character;
using TextRpg.Game.Utilities;
using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities;

namespace TextRpg.Game.Controllers.Menu
{
    public static class CharacterMenuController
    {
        public static string CreateCharacter()
        {
            string characterName = "";
            string race = "";
            string characterClass = "";

            Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", "Character creation process started.");

            while (true)
            {
                characterName = CreateCharacterMenu.GetCharacterName();
                if (string.IsNullOrEmpty(characterName))
                {
                    Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", "Character creation cancelled.");
                    return "";
                }

                Dictionary<string, CharacterModel> characters = CharacterDataService.GetLoadedCharacters();
                if (characters.ContainsKey(characterName))
                {
                    GameWriter.CenterText("A character with this name already exists.");
                    GameWriter.CenterText("\nPress any key to try again...");
                    Console.ReadKey(true);
                    Logger.LogWarning($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", $"Character creation failed: Name '{characterName}' already exists.");
                }
                else
                {
                    Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", $"Character name '{characterName}' selected.");
                    break;
                }
            }

            while (true)
            {
                race = RaceSelectionMenu.Show();
                if (!string.IsNullOrEmpty(race))
                {
                    Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", $"Race '{race}' selected for character '{characterName}'.");
                    break;
                }
            }

            while (true)
            {
                characterClass = ClassSelectionMenu.Show();
                if (!string.IsNullOrEmpty(characterClass))
                {
                    Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", $"Class '{characterClass}' selected for character '{characterName}'.");
                    break;
                }
            }

            bool confirmed = CreateCharacterMenu.ConfirmCharacter(characterName, race, characterClass);
            if (!confirmed)
            {
                Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", $"Character creation cancelled for '{characterName}'.");
                return "";
            }

            Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(CreateCharacter)}", $"Character '{characterName}' successfully created with Race '{race}' and Class '{characterClass}'.");
            CharacterDataService.CreateCharacter(characterName, race, characterClass);

            return characterName;
        }

        public static void DeleteCharacter()
        {
            Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(DeleteCharacter)}", "Character deletion process started.");
            string characterToDelete = DeleteCharacterMenu.Show();

            if (!string.IsNullOrEmpty(characterToDelete))
            {
                Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(DeleteCharacter)}", $"Character '{characterToDelete}' selected for deletion.");
                CharacterDataService.DeleteCharacter(characterToDelete);
                Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(DeleteCharacter)}", $"Character '{characterToDelete}' successfully deleted.");
            }
            else
            {
                Logger.LogInfo($"{nameof(CharacterMenuController)}::{nameof(DeleteCharacter)}", "Character deletion cancelled.");
            }
        }
    }
}
