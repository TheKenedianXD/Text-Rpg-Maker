
namespace TextRpg.Game.Utilities
{
    public static class GameWriter
    {
        public static void CenterText(string text)
        {
            int screenWidth = Console.WindowWidth;
            int padding = (screenWidth - text.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(padding, 0)) + text);
        }
    }
}
