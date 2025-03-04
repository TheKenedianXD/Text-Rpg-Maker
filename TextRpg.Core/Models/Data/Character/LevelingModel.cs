using System.Collections.Generic;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Models.Data.Character
{
    public class LevelingModel
    {
        [Description("The experience points required to reach each level.")]
        public Dictionary<int, int> XpRequirements { get; set; } = [];

        [Description("The stat increases applied per level.")]
        public Dictionary<int, Dictionary<BaseStat, float>> StatIncreases { get; set; } = [];

        public LevelingModel()
        {

        }
    }
}
