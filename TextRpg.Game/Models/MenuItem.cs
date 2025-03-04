namespace TextRpg.Game.Models
{
    public class MenuItem
    {
        public string Name { get; }
        public Action? Action { get; }
        public string? Description { get; }
        public bool IsReturningIndex { get; }

        public MenuItem(string name, Action? action, bool isReturningIndex, string? description = null)
        {
            Name = name;
            Action = action;
            IsReturningIndex = isReturningIndex;
            Description = description;
        }
    }
}