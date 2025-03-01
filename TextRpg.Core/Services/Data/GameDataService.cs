using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Data
{
    public static class GameDataService
    {
        private const string basePath = "Game/Data/";

        private static readonly Dictionary<GameData, (Type Type, string FilePath)> DataFiles = new()
        {
            { GameData.Races, (typeof(RaceModel), $"{basePath}Races.json") },
            { GameData.Classes, (typeof(ClassModel), $"{basePath}Classes.json") },
            { GameData.Stats, (typeof(StatModel), $"{basePath}Stats.json") },
            { GameData.Leveling, (typeof(LevelingModel), $"{basePath}Leveling.json") },
        };

        private static readonly Dictionary<GameData, object> LoadedData = [];

        public static void LoadGameData()
        {
            foreach (var entry in DataFiles)
            {
                var (modelType, filePath) = entry.Value;
                Logger.LogInfo($"{nameof(GameDataService)}::{nameof(LoadGameData)}", $"Loading data from {filePath}");

                object? data = JsonService.Load(modelType, filePath);

                if (data != null)
                {
                    Logger.LogInfo($"{nameof(GameDataService)}::{nameof(LoadGameData)}", $"Successfully loaded data from {filePath}");
                } else
                {
                    Logger.LogWarning($"{nameof(GameDataService)}::{nameof(LoadGameData)}", $"Failed to load data from {filePath}, using default values.");
                }

                LoadedData[entry.Key] = data ?? Activator.CreateInstance(modelType) ?? new object();
            }
        }

        public static List<T> GetData<T>(GameData key) where T : class
        {
            Logger.LogInfo($"{nameof(GameDataService)}::{nameof(GetData)}", $"Retrieving data for {key}");

            if (LoadedData.TryGetValue(key, out object? value) && value is List<T> typedList)
            {
                Logger.LogInfo($"{nameof(GameDataService)}::{nameof(GetData)}", $"Successfully retrieved data for {key}");
                return typedList;
            }

            Logger.LogWarning($"{nameof(GameDataService)}::{nameof(GetData)}", $"Data for {key} not found or is of incorrect type.");
            return [];
        }

        public static T GetSingle<T>(GameData key) where T : class
        {
            Logger.LogInfo($"{nameof(GameDataService)}::{nameof(GetSingle)}", $"Retrieving single data object for {key}");

            if (LoadedData.TryGetValue(key, out object? value) && value is T typedObject)
            {
                Logger.LogInfo($"{nameof(GameDataService)}::{nameof(GetSingle)}", $"Successfully retrieved single data object for {key}");
                return typedObject;
            }

            Logger.LogWarning($"{nameof(GameDataService)}::{nameof(GetSingle)}", $"Data for {key} not found or is of incorrect type.");
            return Activator.CreateInstance<T>();
        }
    }
}
