using TextRpg.Core.Models.Enums;

namespace TextRpg.Core.Models.Base
{
    public class BaseStatsModel
    {
        public Dictionary<BaseStat, float> Stats { get; set; }
        public BaseStatsModel()
        {
            Stats = new Dictionary<BaseStat, float>
            {
                { BaseStat.MaxHealth, 100f },
                { BaseStat.MaxSpecialResource, 50f },
                { BaseStat.MinDamage, 10f },
                { BaseStat.MaxDamage, 20f },
                { BaseStat.Defense, 5f },
                { BaseStat.SpecialDefense, 5f },
                { BaseStat.CritChance, 0.1f },
                { BaseStat.CritDamage, 1.5f },
                { BaseStat.Evasion, 0.05f },
                { BaseStat.SpecialEvasion, 0.05f }
            };
        }
    }
}
