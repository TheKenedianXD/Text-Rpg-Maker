using TextRpg.Game.Menus;
using TextRpg.Game.Menus.Character;
using TextRpg.Game.Utilities;
using TextRpg.Core.Utilities;
using TextRpg.Core.Services.Data;
using TextRpg.Game.Enums;

namespace TextRpg.Game.Controllers.Menu
{
    public static class MenuController
    {
        public static void ShowWelcomeScreen()
        {
            Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowWelcomeScreen)}", "Displaying welcome screen.");
            WelcomeMenu.Show(ShowCharacterSelection, () =>
            {
                Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowWelcomeScreen)}", "Exiting game from Welcome Screen.");
                GameController.SetGameState(MenuState.Exit);
            });

            if (GameController.GetGameState() == MenuState.Exit ||
                GameController.GetGameState() == MenuState.MainMenu)
                return;

            Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowWelcomeScreen)}", "Switching to Character Selection.");
            GameController.SetGameState(MenuState.CharacterSelection);
        }

        public static void ShowCharacterSelection()
        {
            Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowCharacterSelection)}", "Displaying character selection menu.");
            string selectedCharacter = CharacterMenu.Show(() => CharacterMenuController.CreateCharacter(), CharacterMenuController.DeleteCharacter);

            if (!string.IsNullOrEmpty(selectedCharacter))
            {
                Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowCharacterSelection)}", $"Character '{selectedCharacter}' selected.");
                GameController.SetSelectedCharacter(selectedCharacter);
                Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowCharacterSelection)}", "Switching to Main Menu.");
                GameController.SetGameState(MenuState.MainMenu);
            }
            else
            {
                Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowCharacterSelection)}", "Character selection canceled or no character chosen.");
            }
        }

        public static void ShowMainMenu()
        {
            Console.Clear();
            string character = GameController.GetSelectedCharacter();
            Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowMainMenu)}", $"Displaying Main Menu for character '{character}'.");

            GameWriter.CenterText($"Welcome, {character}!");
            GameWriter.CenterText("\nMain menu will be implemented later...");
            GameWriter.CenterText("\nPress any key to exit...");
            Console.ReadKey();

            Logger.LogInfo($"{nameof(MenuController)}::{nameof(ShowMainMenu)}", "Exiting game from Main Menu.");
            GameController.SetGameState(MenuState.Exit);
        }
    }
}