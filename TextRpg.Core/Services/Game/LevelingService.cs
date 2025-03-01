using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Models.Player;
using TextRpg.Core.Services.Data;
using TextRpg.Core.Utilities;

namespace TextRpg.Core.Services.Game
{
    public static class LevelingService
    {
        private static readonly BaseStat[] BaseStats = (BaseStat[])Enum.GetValues(typeof(BaseStat));

        public static int GetXpForLevel(int level)
        {
            var levelingData = GameDataService.GetSingle<LevelingModel>(GameData.Leveling);
            int xpRequired = levelingData != null && levelingData.XpRequirements.TryGetValue(level, out int value)
                ? value : -1;

            Logger.LogInfo($"{nameof(LevelingService)}::{nameof(GetXpForLevel)}",
                levelingData != null
                    ? $"XP required for level {level}: {xpRequired}"
                    : $"Leveling data not found, returning -1 for level {level}");

            return xpRequired;
        }

        public static Dictionary<BaseStat, float> GetStatBonuses(int level)
        {
            var cumulativeMultipliers = new Dictionary<BaseStat, float>();

            foreach (var baseStat in BaseStats)
            {
                cumulativeMultipliers[baseStat] = 1f;
            }

            var levelingData = GameDataService.GetSingle<LevelingModel>(GameData.Leveling);

            for (int i = 2; i <= level; i++)
            {
                if (levelingData != null && levelingData.StatIncreases.TryGetValue(i, out var value))
                {
                    foreach (var statIncrease in value)
                    {
                        cumulativeMultipliers[statIncrease.Key] *= 1 + statIncrease.Value;
                    }
                }
            }

            Logger.LogInfo($"{nameof(LevelingService)}::{nameof(GetStatBonuses)}",
                $"Stat bonuses for level {level}: {string.Join(", ", cumulativeMultipliers.Select(kv => $"{kv.Key}: {kv.Value}"))}");

            return cumulativeMultipliers;
        }

        public static void LevelUp(PlayerModel player)
        {
            while (true)
            {
                int requiredXp = GetXpForLevel(player.Character.Level);
                if (requiredXp == -1 || player.Character.Experience < requiredXp)
                    break;

                Logger.LogInfo($"{nameof(LevelingService)}::{nameof(LevelUp)}",
                    $"Player gained {requiredXp} XP, leveling up from {player.Character.Level} to {player.Character.Level + 1}");

                player.Character.Experience -= requiredXp;
                player.Character.Level += 1;
                Logger.LogInfo($"{nameof(LevelingService)}::{nameof(LevelUp)}",
                    $"Player leveled up! New Level: {player.Character.Level}, Experience: {player.Character.Experience}");
            }

            StatService.RecalculateStats(player.Character);

            ResourceService.ResetHealth(player.Character);
            ResourceService.ResetSpecialResource(player.Character);

            Logger.LogInfo($"{nameof(LevelingService)}::{nameof(LevelUp)}",
                $"Stats and resources reset for {player.Character.GetType().Name} after leveling up.");
        }
    }
}
