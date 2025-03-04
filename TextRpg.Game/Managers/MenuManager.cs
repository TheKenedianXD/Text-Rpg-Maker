using System;
using System.Collections.Generic;
using TextRpg.Core.Utilities;
using TextRpg.Game.Utilities;
using TextRpg.Game.Models;

namespace TextRpg.Game.Managers
{
    public class MenuManager
    {
        private readonly List<MenuItem> _menuItems;
        private int _selectedIndex = 0;

        public MenuManager(List<MenuItem> menuItems)
        {
            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(MenuManager)}", "Initializing menu.");
            _menuItems = new List<MenuItem>(menuItems);
        }

        public int ShowMenu(string header = "", string footer = "")
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
                        Move(-1);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        Move(1);
                        break;
                    case ConsoleKey.Enter:
                        var selectedItem = _menuItems[_selectedIndex];
                        if (selectedItem.Action != null)
                        {
                            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(ShowMenu)}", $"Executing action for menu item: {selectedItem.Name}");
                            selectedItem.Action.Invoke();
                        }
                        return selectedItem.IsReturningIndex ? _selectedIndex : -1;
                }

                if (!string.IsNullOrEmpty(footer))
                {
                    Console.WriteLine();
                    GameWriter.CenterText(footer);
                }
            }
            return -1;
        }

        private void Move(int direction)
        {
            int newIndex = _selectedIndex;

            for (int attempts = 0; attempts < _menuItems.Count; attempts++)
            {
                newIndex += direction;

                // Wrap around manually
                if (newIndex >= _menuItems.Count) newIndex = 0;
                if (newIndex < 0) newIndex = _menuItems.Count - 1;

                // Stop if we find a valid item
                if (_menuItems[newIndex].Action != null)
                {
                    _selectedIndex = newIndex;
                    return;
                }
            }

            Logger.LogWarning($"{nameof(MenuManager)}::{nameof(Move)}", "No valid menu items found.");
        }


        public void AddMenuItems(List<MenuItem> newItems)
        {
            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(AddMenuItems)}", $"Adding {newItems.Count} menu items.");
            _menuItems.AddRange(newItems);
        }

        public void RemoveMenuItems(List<MenuItem> itemsToRemove)
        {
            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(RemoveMenuItems)}", $"Removing {itemsToRemove.Count} menu items.");
            _menuItems.RemoveAll(itemsToRemove.Contains);
        }

        private void RenderMenu()
        {
            Logger.LogInfo($"{nameof(MenuManager)}::{nameof(RenderMenu)}", "Rendering menu.");

            for (int i = 0; i < _menuItems.Count; i++)
            {
                var menuItem = _menuItems[i];

                if (string.IsNullOrWhiteSpace(menuItem.Name) && menuItem.Action == null)
                {
                    Console.WriteLine();
                    continue;
                }

                bool isSelected = (i == _selectedIndex);
                string formattedName = isSelected ? $"[ {menuItem.Name} ]" : $"  {menuItem.Name}  ";

                if (!string.IsNullOrEmpty(menuItem.Description))
                {
                    string[] descriptionLines = menuItem.Description.Split("\n");
                    GameWriter.ColoredCenterText([
                        (formattedName, isSelected ? ConsoleColor.Yellow : null),
                        (" | " + descriptionLines[0], null)
                    ]);

                    for (int j = 1; j < descriptionLines.Length; j++)
                    {
                        GameWriter.CenterText(descriptionLines[j]);
                    }
                } else
                {
                    GameWriter.ColoredCenterText([(formattedName, isSelected ? ConsoleColor.Yellow : null)]);
                }
            }
        }
    }
}