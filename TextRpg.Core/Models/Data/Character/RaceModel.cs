using TextRpg.Core.Models.Base;
using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Models.Data.Character
{
    public class RaceModel
    {
        [Description("The name of the race.")]
        public string Name { get; set; } = "";

        [Description("A brief description of the race, including its background and traits.")]
        public string Description { get; set; } = "";

        [Description("The default base stats assigned to this race.")]
        public BaseStatsModel DefaultBaseStats { get; set; }

        public RaceModel(string name, string description, BaseStatsModel defaultBaseStats)
        {
            Name = name;
            Description = description;
            DefaultBaseStats = defaultBaseStats;
        }
    }
}
