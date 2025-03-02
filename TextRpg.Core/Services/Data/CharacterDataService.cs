using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Data
{
    public static class CharacterDataService
    {
        private const string BasePath = "Game/Saves/";

        private static readonly Dictionary<CharacterData, (Type Type, string FileName)> DataFiles = new()
        {
            { CharacterData.Character, (typeof(CharacterModel), "Character.json") },
            { CharacterData.PrimaryStats, (typeof(PrimaryStatsModel), "PrimaryStats.json") }
        };

        private static readonly Dictionary<CharacterData, object> LoadedData = [];

        public static void LoadCharacterData(string characterName)
        {
            Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(LoadCharacterData)}", $"Loading character data for {characterName}");

            foreach (var entry in DataFiles)
            {
                var (modelType, fileName) = entry.Value;
                string filePath = $"{BasePath}{characterName}/{fileName}";

                object? data = JsonService.Load(modelType, filePath);

                if (data == null)
                {
                    Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(LoadCharacterData)}", $"Failed to load data for {characterName} from {filePath}, using default values.");
                }

                LoadedData[entry.Key] = data ?? Activator.CreateInstance(modelType) ?? new object();
            }
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

        public static void CreateCharacter(string name, string race, string characterClass)
        {
            string path = Path.Combine(BasePath, name);
            if (Directory.Exists(path))
            {
                Logger.LogError(nameof(CharacterDataService), $"Character {name} already exists.");
                return;
            }

            Directory.CreateDirectory(path);
            CharacterModel character = new(name, 0, 1, race, characterClass, DateTime.Now, 0, 0, []);
            JsonService.Save(typeof(CharacterModel), character, Path.Combine(path, "Character.json"));
        }

        public static void DeleteCharacter(string name)
        {
            string path = Path.Combine(BasePath, name);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Logger.LogInfo(nameof(CharacterDataService), $"Deleted character {name}.");
            } else
            {
                Logger.LogError(nameof(CharacterDataService), $"Character {name} not found.");
            }
        }
    }
}
