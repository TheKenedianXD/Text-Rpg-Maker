using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using TextRpg.Core.Utilities;
using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Services.Data
{
    public static class JsonService
    {
#pragma warning disable CA1869
        public static object? Load(Type type, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Logger.LogWarning($"{nameof(JsonService)}::{nameof(Load)}", $"File not found: {filePath}");
                    return Activator.CreateInstance(type);
                }

                var jsonString = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize(jsonString, type, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                             ?? Activator.CreateInstance(type);

                if (result != null)
                {
                    ApplyDefaultValues(result);
                    Logger.LogInfo($"{nameof(JsonService)}::{nameof(Load)}", $"Successfully loaded and deserialized {filePath}");
                }

                return result;
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(JsonService)}::{nameof(Load)}", $"Error loading JSON from {filePath}: {ex.Message}", ex);
                return Activator.CreateInstance(type);
            }
        }

        public static void Save(Type type, object data, string filePath)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath) ?? "";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = JsonSerializer.Serialize(data, type, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonString);

                Logger.LogInfo($"{nameof(JsonService)}::{nameof(Save)}", $"Successfully saved data to {filePath}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(JsonService)}::{nameof(Save)}", $"Error saving JSON to {filePath}: {ex.Message}", ex);
            }
        }

        private static void ApplyDefaultValues(object obj)
        {
            foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetValue(obj) == null)
                {
                    var defaultValue = AttributeHelper.GetDefaultValue(property);
                    property.SetValue(obj, defaultValue);
                    Logger.LogInfo($"{nameof(JsonService)}::{nameof(ApplyDefaultValues)}",
                        $"Applied default value {defaultValue} to {property.Name}");
                }
            }
        }
#pragma warning restore CA1869
    }
}
