using TextRpg.Core.Services.Data;
using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Models.Enums;
using TextRpg.Game.Managers;
using TextRpg.Game.Utilities;
using TextRpg.Core.Utilities;
using TextRpg.Game.Models;

namespace TextRpg.Game.Menus.Character
{
    public static class ClassSelectionMenu
    {
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
                return "";
            }

            List<MenuItem> menuItems = [];
            foreach (var classModel in classes)
            {
                menuItems.Add(new MenuItem(classModel.Name, () => ShowClassDetails(classModel)));
            }

            MenuItem[,] menuArray = new MenuItem[menuItems.Count, 1];
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuArray[i, 0] = menuItems[i];
            }

            MenuManager menu = new(menuArray);
            (int selectedRow, int selectedCol) = menu.ShowMenu("== Select Class ==");

            if (selectedRow >= 0 && selectedCol == 0)
            {
                Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", $"Class '{classes[selectedRow].Name}' selected.");
                return classes[selectedRow].Name;
            }

            Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(Show)}", "No class selected, returning to previous menu.");
            return "";
        }

        private static void ShowClassDetails(ClassModel classModel)
        {
            Logger.LogInfo($"{nameof(ClassSelectionMenu)}::{nameof(ShowClassDetails)}", $"Displaying details for class '{classModel.Name}'.");

            Console.Clear();
            GameWriter.CenterText($"Class: {classModel.Name}");
            Console.WriteLine();
            GameWriter.CenterText(classModel.Description);
            GameWriter.CenterText("\nPress any key to go back...");
            Console.ReadKey(true);
        }
    }
}