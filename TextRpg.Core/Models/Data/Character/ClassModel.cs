using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Models.Data.Character
{
    public class ClassModel
    {
        [Description("The name of the class.")]
        public string Name { get; set; } = "";

        [Description("A brief description of the class, explaining its role and abilities.")]
        public string Description { get; set; } = "";

        public ClassModel()
        {

        }

        public ClassModel(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
