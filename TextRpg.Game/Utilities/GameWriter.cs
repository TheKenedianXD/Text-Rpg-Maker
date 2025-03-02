
namespace TextRpg.Game.Utilities
{
    public static class GameWriter
    {
        public static void CenterText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine();
                return;
            }

            int screenWidth = Console.WindowWidth;

            string[] lines = text.Split('\n');

            foreach (string line in lines)
            {
                string trimmedLine = line.TrimEnd();

                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    Console.WriteLine();
                    continue;
                }

                int padding = (screenWidth - trimmedLine.Length) / 2;
                padding = Math.Max(padding, 0);

                Console.WriteLine(new string(' ', padding) + trimmedLine);
            }
        }


        public static void ColoredCenterText(List<(string text, ConsoleColor? color)> coloredSegments)
        {
            if (coloredSegments == null || coloredSegments.Count == 0)
            {
                Console.WriteLine();
                return;
            }

            int totalLength = coloredSegments.Sum(segment => segment.text.Length);
            int screenWidth = Console.WindowWidth;
            int padding = (screenWidth - totalLength) / 2;
            padding = Math.Max(padding, 0);

            Console.Write(new string(' ', padding));

            foreach (var (text, color) in coloredSegments)
            {
                if (color.HasValue)
                    Console.ForegroundColor = color.Value;

                Console.Write(text);

                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }

}
