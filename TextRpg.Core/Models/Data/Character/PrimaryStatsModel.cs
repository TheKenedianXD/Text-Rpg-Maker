using System.Collections.Generic;
using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Models.Data.Character
{
    public class PrimaryStatsModel
    {
        [Description("The character's primary stats, which influence base stats and abilities.")]
        public Dictionary<string, int> Stats { get; set; } = [];

        public PrimaryStatsModel()
        {
            
        }

        public PrimaryStatsModel(Dictionary<string, int> stats)
        {
            Stats = stats;
        }
    }
}
