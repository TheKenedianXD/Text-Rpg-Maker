using TextRpg.Core.Utilities.Attributes;

namespace TextRpg.Core.Models.Config
{
    public class AppConfigModel
    {
        [Description("Defines the minimum log level to be recorded.")]
        [DefaultValue("Info")]
        public string LogLevel { get; set; } = "Info";

        public AppConfigModel() { }

        public AppConfigModel(string logLevel)
        {
            LogLevel = logLevel;
        }
    }
}
