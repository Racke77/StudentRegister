using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegister
{
    internal class Menu
    {
        public List<string> MenuList;
        public int MenuSelect;
        public Boolean SelectedOption;
        public Menu()
        {
            StartingMenu();
        }
        #region Updating Menu List
        public void StartingMenu()
        {
            if (MenuList != null)
            {
                MenuList.Clear();
            }
            MenuList = new List<string>()
            {
                "Display all students",
                "Add new student",
                "Edit student" ,
                "Find students",
                "Exit"
            };
        }
        public void MenuUpdate(List<string> menuOptions)
        {
            MenuList = menuOptions;
        }
        #endregion
        public int MenuSelection()
        {
            MenuSelect = 0;
            SelectedOption = false;
            while (SelectedOption != true)
            {
                Console.Clear();
                Console.CursorVisible = false;                
                PrintMenuOptions();//printing out the full menu
                HandleUserInput();
            }
            return MenuSelect;
        }
        #region Printing
        private void PrintMenuOptions()
        {
            foreach (string menuOption in MenuList)
            {
                if (MenuList.IndexOf(menuOption) == MenuSelect)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(menuOption);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        public int PrintMenuForEdit(Student student, School school)
        {
            MenuSelect = 0;
            SelectedOption = false;
            while (SelectedOption != true)
            {
                Console.Clear();
                school.PrintStudent(student);
                Console.WriteLine();
                Console.CursorVisible = false;
                PrintMenuOptions();//printing out the full menu
                HandleUserInput();
            }
            return MenuSelect;
        }
        #endregion
        #region User-input
        private void HandleUserInput()
        {
            var keyPressed = Console.ReadKey();

            switch (keyPressed.Key)
            {
                case ConsoleKey.DownArrow:
                    //when pressing down -> add+1 -> residuals of new value and menu.count
                    //if menu.count is bigger than value -> full new value is always the residual
                    //if menu.count is equal to value -> the new value becomes 0 (loop back to beginning)
                    MenuSelect = (MenuSelect + 1) % MenuList.Count;
                    break;
                case ConsoleKey.UpArrow:
                    //when pressing up -> if start of menu-option -> loop to last menu-option -> ELSE go up
                    if (MenuSelect == 0)
                    {
                        MenuSelect = MenuList.Count - 1;
                    }
                    else
                    {
                        MenuSelect--;
                    }
                    break;
                    //when pressing enter -> update property to true -> break the loop
                case ConsoleKey.Enter:
                    SelectedOption = true;
                    break;
            }
        }
        #endregion
    }
}
