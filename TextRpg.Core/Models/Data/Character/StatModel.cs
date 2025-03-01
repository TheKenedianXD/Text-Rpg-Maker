using System.Collections.Generic;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Models.Data.Character
{
    public class StatModel
    {
        [Description("The name of the stat (e.g., Strength, Dexterity, Stamina).")]
        public string Name { get; set; } = "";

        [Description("Indicates whether increasing this stat is beneficial.")]
        public bool Increases { get; set; }

        [Description("A dictionary that maps affected base stats to their effect value and type.")]
        public Dictionary<BaseStat, (float Value, bool IsMultiplier)> Affects { get; set; } = [];

        public StatModel(string name, bool increases, Dictionary<BaseStat, (float, bool)> affects)
        {
            Name = name;
            Increases = increases;
            Affects = affects;
        }
    }
}