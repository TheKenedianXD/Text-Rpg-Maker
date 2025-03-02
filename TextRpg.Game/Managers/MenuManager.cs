using TextRpg.Core.Utilities;
using TextRpg.Game.Models;
using TextRpg.Game.Utilities;

namespace TextRpg.Game.Managers
{
    public class MenuManager
    {
        private readonly MenuItem[,] _menuItems;
        private readonly int _rows, _cols;
        private int _selectedRow = 0, _selectedCol = 0;

        public MenuManager(MenuItem[,] menuItems)
        {
            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(MenuManager)}", "Initializing menu.");
            _menuItems = menuItems;
            _rows = _menuItems.GetLength(0);
            _cols = _menuItems.GetLength(1);

            Move(0, 0);
        }

        public (int, int) ShowMenu(string header = "", string footer = "")
        {
            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(ShowMenu)}", "Displaying menu.");
            bool running = true;

            while (running)
            {
                Console.Clear();

                if (!string.IsNullOrEmpty(header))
                {
                    GameWriter.CenterText(header);
                    Console.WriteLine();
                }

                RenderMenu();

                var key = Console.ReadKey(true).Key;
                Logger.LogInfo($"{nameof(MenuManager)}::{nameof(ShowMenu)}", $"Key pressed: {key}");

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        Move(-1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        Move(1, 0);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        Move(0, -1);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        Move(0, 1);
                        break;
                    case ConsoleKey.Enter:
                        var selectedItem = _menuItems[_selectedRow, _selectedCol];
                        if (selectedItem != null)
                        {
                            if (selectedItem.Action != null)
                            {
                                Logger.LogInfo($"{nameof(MenuManager)}::{nameof(ShowMenu)}", $"Executing action for menu item: {selectedItem.Name}");
                                selectedItem.Action.Invoke();
                                return (-1, -1);
                            }
                            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(ShowMenu)}", $"Menu item selected: {_selectedRow}, {_selectedCol}");
                            return (_selectedRow, _selectedCol);
                        }
                        break;
                }

                if (!string.IsNullOrEmpty(footer))
                {
                    Console.WriteLine();
                    GameWriter.CenterText(footer);
                }
            }
            return (-1, -1);
        }

        private void Move(int rowOffset, int colOffset)
        {
            int newRow = _selectedRow;
            int newCol = _selectedCol;

            do
            {
                newRow = (newRow + rowOffset + _rows) % _rows;
                newCol = (newCol + colOffset + _cols) % _cols;
            }
            while (_menuItems[newRow, newCol] == null || _menuItems[newRow, newCol].Action == null);

            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(Move)}", $"Cursor moved to: {newRow}, {newCol}");

            _selectedRow = newRow;
            _selectedCol = newCol;
        }

        private void RenderMenu()
        {
            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(RenderMenu)}", "Rendering menu.");

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _cols; col++)
                {
                    if (_menuItems[row, col] == null)
                    {
                        Console.Write("        ");
                        continue;
                    }

                    if (_menuItems[row, col].Action == null)
                    {
                        Console.WriteLine();
                        continue;
                    }

                    if (row == _selectedRow && col == _selectedCol)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"[ {_menuItems[row, col].Name} ]");
                        Console.ResetColor();
                    } else
                    {
                        Console.Write($"  {_menuItems[row, col].Name}  ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
