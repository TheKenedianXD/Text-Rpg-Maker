using System;
using System.Collections.Generic;
using TextRpg.Core.Models.Entities;
using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Models.Data.Character
{
    public class CharacterModel : LivingEntityBase
    {
        [Description("The name of the character.")]
        public string Name { get; set; } = "";

        [Description("The amount of experience points accumulated by the character.")]
        [DefaultValue(0)]
        public float Experience { get; set; }

        [Description("The level of the character.")]
        [DefaultValue(1)]
        public int Level { get; set; }

        [Description("The race of the character.")]
        public string Race { get; set; } = "";

        [Description("The character's class, which determines abilities and playstyle.")]
        public string CharacterClass { get; set; } = "";

        [Description("The last time this character was played.")]
        [DefaultValue("NOW")] // ✅ Special case: Set dynamically
        public DateTime LastPlayed { get; set; }

        [Description("The character's primary stats, used for calculations.")]
        public Dictionary<string, int> PrimaryStats { get; set; } = [];

        public CharacterModel(string name, float experience, int level, string race, string characterClass, DateTime lastPlayed,
            float currentHealth, float currentSpecialResource, Dictionary<string, int> primaryStats)
            : base(currentHealth, currentSpecialResource)
        {
            Name = name;
            Experience = experience;
            Level = level;
            Race = race;
            CharacterClass = characterClass;
            LastPlayed = lastPlayed;
            PrimaryStats = primaryStats;
        }
    }
}
