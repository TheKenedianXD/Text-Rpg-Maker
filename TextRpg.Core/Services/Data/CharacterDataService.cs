using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Data
{
    public static class CharacterDataService
    {
        private const string BasePath = "Saves\\";

        private static readonly Dictionary<CharacterData, (Type Type, string FileName)> DataFiles = new()
        {
            { CharacterData.Character, (typeof(CharacterModel), "Character.json") }
        };

        private static readonly Dictionary<CharacterData, object> LoadedData = [];

        private static readonly Dictionary<string, CharacterModel> LoadedCharacters = [];

        public static void LoadCharacterData(string characterName)
        {
            Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(LoadCharacterData)}", $"Loading character data for {characterName}");

            foreach (var entry in DataFiles)
            {
                var (modelType, fileName) = entry.Value;
                string filePath = $"{BasePath}{characterName}\\{fileName}";

                object? data = JsonService.Load(modelType, filePath);

                if (data == null)
                {
                    Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(LoadCharacterData)}", $"Failed to load data for {characterName} from {filePath}, using default values.");
                }

                LoadedData[entry.Key] = data ?? Activator.CreateInstance(modelType) ?? new object();
            }
        }

        public static void LoadAllCharacters()
        {
            Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(LoadAllCharacters)}", "Loading all character data.");

            LoadedCharacters.Clear();

            string gamePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Game/"));
            string savesPath = gamePath + BasePath;

            if (Directory.Exists(savesPath))
            {
                foreach (var characterFolder in Directory.GetDirectories(savesPath))
                {
                    string characterName = Path.GetFileName(characterFolder);
                    string characterFile = Path.Combine(characterFolder, "Character.json");

                    if (File.Exists(characterFile))
                    {
                        CharacterModel? character = JsonService.Load<CharacterModel>(characterFile);

                        if (character != null)
                        {
                            LoadedCharacters[characterName] = character;
                            Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(LoadAllCharacters)}", $"Loaded character: {characterName}");
                        } else
                        {
                            Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(LoadAllCharacters)}", $"Failed to load character: {characterName}");
                        }
                    } else
                    {
                        Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(LoadAllCharacters)}", $"Character file missing: {characterFile}");
                    }
                }
            } else
            {
                Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(LoadAllCharacters)}", "No character saves found.");
                return;
            }

            Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(LoadAllCharacters)}", $"Loaded {LoadedCharacters.Count} characters.");
        }




        public static List<T> GetData<T>(CharacterData key) where T : class
        {
            Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(GetData)}", $"Retrieving data for {key}");

            if (LoadedData.TryGetValue(key, out object? value) && value is List<T> typedList)
                return typedList;

            if (DataFiles.ContainsKey(key))
            {
                Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(GetData)}", $"Data for {key} not found or is of incorrect type.");
            }
            return [];
        }

        public static T GetSingle<T>(CharacterData key) where T : class
        {
            Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(GetSingle)}", $"Retrieving single data object for {key}");

            if (LoadedData.TryGetValue(key, out object? value) && value is T typedObject)
                return typedObject;

            if (DataFiles.ContainsKey(key))
            {
                Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(GetSingle)}", $"Data for {key} not found or is of incorrect type.");
            }
            return Activator.CreateInstance<T>();
        }

        public static Dictionary<string, CharacterModel> GetLoadedCharacters()
        {
            return LoadedCharacters;
        }

        public static void CreateCharacter(string name, string race, string characterClass)
        {
            string gamePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"Game/{BasePath}/{name}"));
            string path = Path.Combine(BasePath, name);
            if (Directory.Exists(gamePath))
            {
                Logger.LogError(nameof(CharacterDataService), $"Character {name} already exists.");
                return;
            }

            Directory.CreateDirectory(gamePath);

            var basePrimaryStats = GameDataService.GetData<StatModel>(GameData.Stats); 

            Dictionary<string, int> primaryStats = [];

            foreach (StatModel stats in basePrimaryStats)
            {
                primaryStats.Add(stats.Name, 1);
            }

            CharacterModel character = new(name, 0, 1, race, characterClass, 0, 0, primaryStats);
            JsonService.Save(typeof(CharacterModel), character, Path.Combine(path, "Character.json"));
            LoadAllCharacters();
        }

        public static void DeleteCharacter(string name)
        {
            string gamePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"Game/{BasePath}/{name}"));
            if (Directory.Exists(gamePath))
            {
                Directory.Delete(gamePath, true);
                Logger.LogInfo(nameof(CharacterDataService), $"Deleted character {name}.");
                LoadAllCharacters();
            } 
            else
            {
                Logger.LogError(nameof(CharacterDataService), $"Character {name} not found.");
            }
        }
    }
}
