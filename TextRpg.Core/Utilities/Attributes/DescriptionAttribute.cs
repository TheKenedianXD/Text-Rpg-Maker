
namespace TextRpg.Core.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DescriptionAttribute : Attribute
    {
        public string Text { get; }

        public DescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
