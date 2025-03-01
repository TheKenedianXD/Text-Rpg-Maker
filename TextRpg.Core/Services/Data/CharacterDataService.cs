using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Data
{
    public static class CharacterDataService
    {
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
                string filePath = $"Game/Saves/{characterName}/{fileName}";

                Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(LoadCharacterData)}", $"Attempting to load data from {filePath}");

                object? data = JsonService.Load(modelType, filePath);

                if (data != null)
                {
                    Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(LoadCharacterData)}", $"Successfully loaded data for {characterName} from {filePath}");
                } else
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
            {
                Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(GetData)}", $"Successfully retrieved data for {key}");
                return typedList;
            }

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
            {
                Logger.LogInfo($"{nameof(CharacterDataService)}::{nameof(GetSingle)}", $"Successfully retrieved single data object for {key}");
                return typedObject;
            }

            if (DataFiles.ContainsKey(key))
            {
                Logger.LogWarning($"{nameof(CharacterDataService)}::{nameof(GetSingle)}", $"Data for {key} not found or is of incorrect type.");
            }
            return Activator.CreateInstance<T>();
        }
    }
}
