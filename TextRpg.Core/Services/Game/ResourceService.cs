using TextRpg.Core.Models.Entities;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Game
{
    public static class ResourceService
    {
        public static void ResetHealth(LivingEntityBase entity)
        {
            entity.CurrentHealth = entity.CalculatedStats[BaseStat.MaxHealth];
            Logger.LogInfo($"{nameof(ResourceService)}::{nameof(ResetHealth)}", $"Health was reset for {entity.GetType().Name}");
        }

        public static void ResetSpecialResource(LivingEntityBase entity)
        {
            entity.CurrentSpecialResource = entity.CalculatedStats[BaseStat.MaxSpecialResource];
            Logger.LogInfo($"{nameof(ResourceService)}::{nameof(ResetSpecialResource)}", $"Special Resource was reset for {entity.GetType().Name}");
        }

        public static void AddHealth(LivingEntityBase entity, float amount)
        {
            entity.CurrentHealth += amount;
            if (entity.CurrentHealth > entity.CalculatedStats[BaseStat.MaxHealth])
                entity.CurrentHealth = entity.CalculatedStats[BaseStat.MaxHealth];

            Logger.LogInfo($"{nameof(ResourceService)}::{nameof(AddHealth)}", $"Added health for {entity.GetType().Name} new value: {entity.CurrentHealth}");
        }

        public static void AddSpecialResource(LivingEntityBase entity, float amount)
        {
            entity.CurrentSpecialResource += amount;
            if (entity.CurrentSpecialResource > entity.CalculatedStats[BaseStat.MaxSpecialResource])
                entity.CurrentSpecialResource = entity.CalculatedStats[BaseStat.MaxSpecialResource];

            Logger.LogInfo($"{nameof(ResourceService)}::{nameof(AddSpecialResource)}", $"Added Special Resource for {entity.GetType().Name} new value: {entity.CurrentSpecialResource}");
        }

        public static bool RemoveHealth(LivingEntityBase entity, float amount)
        {
            entity.CurrentHealth -= amount;
            if (entity.CurrentHealth <= 0)
            {
                entity.CurrentHealth = 0;
                Logger.LogInfo($"{nameof(ResourceService)}::{nameof(RemoveHealth)}", $"Removed health for {entity.GetType().Name}, it died");
                return false;
            }
            Logger.LogInfo($"{nameof(ResourceService)}::{nameof(RemoveHealth)}", $"Removed health for {entity.GetType().Name}, new value: {entity.CurrentHealth}");
            return true;
        }

        public static bool RemoveSpecialResource(LivingEntityBase entity, float amount)
        {
            if (amount > entity.CurrentSpecialResource)
            {
                Logger.LogInfo($"{nameof(ResourceService)}::{nameof(RemoveSpecialResource)}", $"{entity.GetType().Name} tried to use an ability with not enough resource");
                return false;
            }

            entity.CurrentSpecialResource -= amount;
            Logger.LogInfo($"{nameof(ResourceService)}::{nameof(RemoveSpecialResource)}", $"{entity.GetType().Name} casted an ability, new value: {entity.CurrentSpecialResource}");
            return true;
        }
    }
}
