namespace TextRpg.Core.Models.Config
{
    public class NamesModel
    {
        public string SpecialResource { get; set; } = "Special Resource";

        public NamesModel()
        {

        }

        public NamesModel(string specialResource)
        {
            SpecialResource = specialResource;
        }
    }
}
