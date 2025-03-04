using System;
using System.Collections.Generic;
using TextRpg.Game.Models;
using TextRpg.Game.Managers;
using TextRpg.Core.Utilities;
using TextRpg.Game.Enums;

namespace TextRpg.Game.Menus.Components
{
    public class ConfirmationComponent
    {
        private readonly List<MenuItem> _confirmationItems;

        public ConfirmationComponent()
        {
            _confirmationItems =
            [
                new MenuItem(nameof(ConfirmationOption.Confirm), () => {}, true),
                new MenuItem(nameof(ConfirmationOption.Cancel), () => {}, true)
            ];
        }

        public void AddToMenu(List<MenuItem> menuItems)
        {
            menuItems.AddRange(_confirmationItems);
        }
        
        public static bool HandleSelection(int selectedIndex, List<MenuItem> menuItems)
        {
            string selectedName = menuItems[selectedIndex].Name;
            return selectedName == nameof(ConfirmationOption.Confirm);
        }
    }
}
