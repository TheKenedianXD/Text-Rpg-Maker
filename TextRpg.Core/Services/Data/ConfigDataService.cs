using TextRpg.Core.Models.Config;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Data
{
    public static class ConfigDataService
    {
        private const string BasePath = "Data/Configs/";

        private static readonly Dictionary<ConfigData, (Type Type, string FilePath)> DataFiles = new()
        {
            { ConfigData.Names, (typeof(NamesModel), $"{BasePath}Names.json") },
            { ConfigData.AppConfig, (typeof(AppConfigModel), $"{BasePath}AppConfig.json") },
        };

        private static readonly Dictionary<ConfigData, object> LoadedData = [];

        public static void LoadConfig()
        {
            Logger.LogInfo($"{nameof(ConfigDataService)}::{nameof(LoadConfig)}", "Loading configuration data.");

            foreach (var entry in DataFiles)
            {
                var (modelType, filePath) = entry.Value;

                object? data = JsonService.Load(modelType, filePath);

                if (data == null)
                {
                    Logger.LogWarning($"{nameof(ConfigDataService)}::{nameof(LoadConfig)}", $"Failed to load {entry.Key} from {filePath}, using default values.");
                }

                LoadedData[entry.Key] = data ?? Activator.CreateInstance(modelType) ?? new object();
            }
        }

        public static List<T> GetData<T>(ConfigData key) where T : class
        {
            Logger.LogInfo($"{nameof(ConfigDataService)}::{nameof(GetData)}", $"Retrieving data for {key}");

            if (LoadedData.TryGetValue(key, out object? value) && value is List<T> typedList)
                return typedList;

            Logger.LogWarning($"{nameof(ConfigDataService)}::{nameof(GetData)}", $"Data for {key} not found or is of incorrect type.");
            return [];
        }

        public static T GetSingle<T>(ConfigData key) where T : class
        {
            Logger.LogInfo($"{nameof(ConfigDataService)}::{nameof(GetSingle)}", $"Retrieving single config data for {key}");

            if (LoadedData.TryGetValue(key, out object? value) && value is T typedObject)
                return typedObject;

            if (DataFiles.ContainsKey(key))
            {
                Logger.LogWarning($"{nameof(ConfigDataService)}::{nameof(GetSingle)}", $"Config data for {key} not found or is of incorrect type.");
            }
            return Activator.CreateInstance<T>();
        }
    }
}
