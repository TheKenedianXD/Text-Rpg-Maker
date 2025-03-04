using TextRpg.Core.Services.Data;
using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Game.Managers;
using TextRpg.Game.Utilities;
using TextRpg.Core.Utilities;
using TextRpg.Game.Models;
using TextRpg.Game.Menus.Components;

namespace TextRpg.Game.Menus.Character
{
    public static class ClassSelectionMenu
    {
        private static string SelectedClass = "";

        public static string Show()
        {
            Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", "Displaying Class Selection menu.");
            Console.Clear();

            List<ClassModel> classes = [];
            try
            {
                classes = GameDataService.GetData<ClassModel>(GameData.Classes);
                Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", $"Loaded {classes.Count} class(es).");
            } catch (Exception ex)
            {
                Logger.LogError($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", "Failed to load class data.", ex);
                return "";
            }

            if (classes.Count == 0)
            {
                Logger.LogWarning($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", "No classes available for selection.");
                DataMissingInfo.ConsiderAddingData("Classes");
                return "";
            }

            List<MenuItem> menuItems = [];
            foreach (var classModel in classes)
            {
                menuItems.Add(new MenuItem(classModel.Name, () =>
                {
                    if (ShowClassDetails(classModel))
                    {
                        SelectedClass = classModel.Name;
                    }
                    else
                    {
                        SelectedClass = "";
                    }
                }, false));
            }

            MenuManager menu = new(menuItems);
            menu.ShowMenu("== Select Class ==");

            if (!string.IsNullOrEmpty(SelectedClass))
            {
                Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", $"Class '{SelectedClass}' selected.");
                return SelectedClass;
            }

            Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", "No class selected, returning to previous menu.");
            return "";
        }

        private static bool ShowClassDetails(ClassModel classModel)
        {
            Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(ShowClassDetails)}", $"Displaying details for class '{classModel.Name}'.");

            Console.Clear();

            List<MenuItem> menuItems = [];
            ConfirmationComponent confirmationMenu = new();
            confirmationMenu.AddToMenu(menuItems);

            MenuManager menu = new(menuItems);
            int selectedIndex = menu.ShowMenu($"Class: {classModel.Name}\n" +
                classModel.Description + 
                "\n");

            bool confirmed = ConfirmationComponent.HandleSelection(selectedIndex, menuItems);

            Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(ShowClassDetails)}",
                $"Class selection confirmation result: {(confirmed ? "Confirmed" : "Cancelled")}");

            return confirmed;
        }
    }
}