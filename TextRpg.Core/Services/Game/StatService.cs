using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Entities;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Services.Data;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Game
{
    public static class StatService
    {
        public static void Initialize(LivingEntityBase entity)
        {
            Logger.LogInfo($"{nameof(StatService)}::{nameof(Initialize)}", $"Initializing stats for {entity.GetType().Name}");
            entity.SetCalculatedStats(CalculateFinalStats(entity));
        }

        public static float GetStat(LivingEntityBase entity, BaseStat stat)
        {
            if (entity.CalculatedStats.TryGetValue(stat, out float value))
            {
                Logger.LogInfo($"{nameof(StatService)}::{nameof(GetStat)}", $"Retrieved {stat} for {entity.GetType().Name}: {value}");
                return value;
            }

            Logger.LogWarning($"{nameof(StatService)}::{nameof(GetStat)}", $"Stat {stat} not found for {entity.GetType().Name}, returning 0.");
            return 0;
        }

        public static Dictionary<BaseStat, float> GetAllStats(LivingEntityBase entity)
        {
            Logger.LogInfo($"{nameof(StatService)}::{nameof(GetAllStats)}", $"Retrieving all stats for {entity.GetType().Name}");
            return new Dictionary<BaseStat, float>(entity.CalculatedStats);
        }

        private static Dictionary<BaseStat, float> CalculateFinalStats(LivingEntityBase entity)
        {
            Logger.LogInfo($"{nameof(StatService)}::{nameof(CalculateFinalStats)}", $"Calculating stats for {entity.GetType().Name}");

            Dictionary<BaseStat, float> baseStats;
            if (entity is CharacterModel character)
            {
                Logger.LogInfo($"{nameof(StatService)}::{nameof(CalculateCharacterStats)}", $"Calculating character stats for {character.Name}");
                baseStats = CalculateCharacterStats(character);
            }
            else
            {
                Logger.LogInfo($"{nameof(StatService)}::{nameof(CalculateFinalStats)}", $"Using predefined stats for {entity.GetType().Name}");
                baseStats = new Dictionary<BaseStat, float>(entity.Stats);
            }

            // Apply modifiers (buffs, etc.)
            // ApplyModifiers(entity, baseStats); TODO
            return baseStats;
        }

        private static Dictionary<BaseStat, float> CalculateCharacterStats(CharacterModel character)
        {
            var raceModel = GameDataService.GetSingle<RaceModel>(GameData.Races);
            var baseStats = new Dictionary<BaseStat, float>(raceModel.DefaultBaseStats.Stats);

            var levelMultipliers = LevelingService.GetStatBonuses(character.Level);
            var primaryStats = character.PrimaryStats;

            var statDefinitions = GameDataService.GetData<StatModel>(GameData.Stats);

            foreach (var stat in baseStats.Keys.ToList())
            {
                if (levelMultipliers.TryGetValue(stat, out float levelMultiplier))
                {
                    baseStats[stat] *= levelMultiplier;
                }
            }

            foreach (var primaryStat in primaryStats)
            {
                if (primaryStat.Value <= 1) continue;

                var statDefinition = statDefinitions.FirstOrDefault(s => s.Name == primaryStat.Key);
                if (statDefinition != null)
                {
                    foreach (var affectedStat in statDefinition.Affects.OrderBy(a => a.Value.IsMultiplier))
                    {
                        if (baseStats.ContainsKey(affectedStat.Key))
                        {
                            float value = primaryStat.Value * affectedStat.Value.Value;

                            if (affectedStat.Value.IsMultiplier)
                            {
                                if (affectedStat.Value.Value >= 0)
                                {
                                    baseStats[affectedStat.Key] *= (1 + value);
                                } 
                                else
                                {
                                    baseStats[affectedStat.Key] /= (1 + Math.Abs(value));
                                }
                            } 
                            else
                            {
                                baseStats[affectedStat.Key] += value;
                            }
                        }
                    }

                }
            }

            Logger.LogInfo($"{nameof(StatService)}::{nameof(CalculateCharacterStats)}",
                $"Final stats for {character.Name}: {string.Join(", ", baseStats.Select(kv => $"{kv.Key}: {kv.Value}"))}");

            return baseStats;
        }


        public static void RecalculateStats(LivingEntityBase entity)
        {
            Logger.LogInfo($"{nameof(StatService)}::{nameof(RecalculateStats)}", $"Recalculating stats for {entity.GetType().Name}");

            if (entity is CharacterModel || NeedsRecalculation(entity))
            {
                entity.SetCalculatedStats(CalculateFinalStats(entity));
            }
        }

#pragma warning disable IDE0060 // for now until implemented TODO
        private static bool NeedsRecalculation(LivingEntityBase entity)
        {
            // Placeholder for future logic (e.g., buffs, debuffs, etc.)
            return false;
        }
#pragma warning restore IDE0060
    }
}
