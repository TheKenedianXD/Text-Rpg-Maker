using TextRpg.Game.Models.Enums;
using TextRpg.Core.Services.Data;
using TextRpg.Game.Controllers.Menu;
using TextRpg.Core.Utilities;
using TextRpg.Core.Models.Config;
using TextRpg.Core.Models.Enums;

namespace TextRpg.Game
{
    public static class GameController
    {
        private static MenuState _currentState = MenuState.WelcomeScreen;
        private static string _selectedCharacter = "";

        public static void Start()
        {
            AppConfigModel config = ConfigDataService.GetSingle<AppConfigModel>(ConfigData.AppConfig);
            Logger.Initialize(config);

            Logger.LogInfo($"{nameof(GameController)}::{nameof(Start)}", "Game started. Loading configuration and game data.");
            ConfigDataService.LoadConfig();
            GameDataService.LoadGameData();
            Logger.LogInfo($"{nameof(GameController)}::{nameof(Start)}", "Configuration and game data loaded. Starting game loop.");
            RunGameLoop();
        }

        private static void RunGameLoop()
        {
            bool running = true;
            while (running)
            {
                Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", $"Current game state: {_currentState}");

                switch (_currentState)
                {
                    case MenuState.WelcomeScreen:
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", "Showing Welcome Screen.");
                        MenuController.ShowWelcomeScreen();
                        break;
                    case MenuState.CharacterSelection:
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", "Showing Character Selection.");
                        MenuController.ShowCharacterSelection();
                        break;
                    case MenuState.CreateCharacter:
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", "Starting Character Creation.");
                        _selectedCharacter = CharacterMenuController.CreateCharacter();
                        _currentState = string.IsNullOrEmpty(_selectedCharacter) ? MenuState.CharacterSelection : MenuState.MainMenu;
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}",
                            string.IsNullOrEmpty(_selectedCharacter)
                                ? "Character creation canceled. Returning to Character Selection."
                                : $"Character '{_selectedCharacter}' created. Moving to Main Menu.");
                        break;
                    case MenuState.DeleteCharacter:
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", "Deleting Character.");
                        CharacterMenuController.DeleteCharacter();
                        _currentState = MenuState.CharacterSelection;
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", "Returning to Character Selection after deletion.");
                        break;
                    case MenuState.MainMenu:
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", "Showing Main Menu.");
                        MenuController.ShowMainMenu();
                        break;
                    case MenuState.Exit:
                        Logger.LogInfo($"{nameof(GameController)}::{nameof(RunGameLoop)}", "Exiting game.");
                        running = false;
                        break;
                }
            }
        }

        public static void SetGameState(MenuState newState)
        {
            Logger.LogInfo($"{nameof(GameController)}::{nameof(SetGameState)}", $"Game state changed from {_currentState} to {newState}");
            _currentState = newState;
        }

        public static MenuState GetGameState()
        {
            return _currentState;
        }

        public static void SetSelectedCharacter(string name)
        {
            Logger.LogInfo($"{nameof(GameController)}::{nameof(SetSelectedCharacter)}", $"Selected character set to '{name}'");
            _selectedCharacter = name;
        }

        public static string GetSelectedCharacter()
        {
            return _selectedCharacter;
        }
    }
}
