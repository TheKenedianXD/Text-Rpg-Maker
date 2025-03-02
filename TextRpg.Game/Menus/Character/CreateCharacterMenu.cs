using TextRpg.Core.Utilities;
using TextRpg.Game.Utilities;

namespace TextRpg.Game.Menus.Character
{
    public static class CreateCharacterMenu
    {
        public static string GetCharacterName()
        {
            Logger.LogInfo($"{nameof(CreateCharacterMenu)}::{nameof(GetCharacterName)}", "Prompting for character name.");

            Console.Clear();
            GameWriter.CenterText("Enter Character Name");
            Console.Write("> ");
            string name = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Logger.LogInfo($"{nameof(CreateCharacterMenu)}::{nameof(GetCharacterName)}", "No name entered, returning empty.");
                return "";
            }

            Logger.LogInfo($"{nameof(CreateCharacterMenu)}::{nameof(GetCharacterName)}", $"Character name entered: {name}");
            return name;
        }

        public static bool ConfirmCharacter(string name, string race, string characterClass)
        {
            Logger.LogInfo($"{nameof(CreateCharacterMenu)}::{nameof(ConfirmCharacter)}", $"Confirming character: Name='{name}', Race='{race}', Class='{characterClass}'");

            string header = $"Are you satisfied with your character?\n\n" +
                            $"Name: {name}\n" +
                            $"Race: {race}\n" +
                            $"Class: {characterClass}\n";

            bool confirmed = ConfirmationMenu.Show(header);
            Logger.LogInfo($"{nameof(CreateCharacterMenu)}::{nameof(ConfirmCharacter)}", $"Character confirmation result: {(confirmed ? "Confirmed" : "Cancelled")}");
            return confirmed;
        }
    }
}
