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
        public static object? Load(Type type, string filePath) // TODO: TEST WHY JSON DO NOT WORK, TEST FROM 
                                                               // Debug, Release, Publish
        {
            try
            {
                string basePath = GetBasePath();
                filePath = Path.Combine(basePath, filePath);

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

        public static T Load<T>(string filePath) where T : class, new()
        {
            try
            {
                string basePath = GetBasePath();
                filePath = Path.Combine(basePath, filePath);

                if (!File.Exists(filePath))
                {
                    Logger.LogWarning($"{nameof(JsonService)}::{nameof(Load)}", $"File not found: {filePath}");
                    return new T();
                }

                var jsonString = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new T();

                ApplyDefaultValues(result);
                Logger.LogInfo($"{nameof(JsonService)}::{nameof(Load)}", $"Successfully loaded and deserialized {filePath}");

                return result;
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(JsonService)}::{nameof(Load)}", $"Error loading JSON from {filePath}: {ex.Message}", ex);
                return new T();
            }
        }

        public static void Save(Type type, object data, string filePath)
        {
            try
            {
                string basePath = GetBasePath();
                filePath = Path.Combine(basePath, filePath);

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
            if (obj == null)
            {
                Logger.LogError($"{nameof(JsonService)}::{nameof(ApplyDefaultValues)}", "Cannot apply default values because object is null.");
                return;
            }

            foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    if (property.GetIndexParameters().Length > 0)
                    {
                        Logger.LogWarning($"{nameof(JsonService)}::{nameof(ApplyDefaultValues)}", $"Skipping indexer property {property.Name}.");
                        continue;
                    }

                    var value = property.GetValue(obj);

                    if (value != null)
                    {
                        Logger.LogInfo($"{nameof(JsonService)}::{nameof(ApplyDefaultValues)}",
                            $"Skipping {property.Name}, already initialized.");
                        continue;
                    }

                    var defaultValue = AttributeHelper.GetDefaultValue(property);

                    if (defaultValue != null && property.PropertyType.IsAssignableFrom(defaultValue.GetType()))
                    {
                        property.SetValue(obj, defaultValue);
                        Logger.LogInfo($"{nameof(JsonService)}::{nameof(ApplyDefaultValues)}",
                            $"Applied default value {defaultValue} to {property.Name}");
                    }
                } catch (TargetParameterCountException)
                {
                    Logger.LogError($"{nameof(JsonService)}::{nameof(ApplyDefaultValues)}",
                        $"Error applying default values: Property {property.Name} requires parameters and cannot be accessed.");
                } catch (Exception ex)
                {
                    Logger.LogError($"{nameof(JsonService)}::{nameof(ApplyDefaultValues)}",
                        $"Error applying default value to {property.Name}: {ex.Message}", ex);
                }
            }
        }

        private static string GetBasePath()
        {
            string gamePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Game/"));

            if (Directory.Exists(gamePath)) return gamePath;

            throw new DirectoryNotFoundException("Game folder not found!");
        }

#pragma warning restore CA1869
    }
}
