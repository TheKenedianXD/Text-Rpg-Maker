
namespace TextRpg.Game.Utilities
{
    public static class DataMissingInfo
    {
        public static void ConsiderAddingData(string name)
        {
            GameWriter.CenterText($"No {name} found. Consider copying BaseData into Data or creating your own!");
            GameWriter.CenterText("Press any key to exit the game...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
