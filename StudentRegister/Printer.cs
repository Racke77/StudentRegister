using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegister
{
    internal class Printer
    {
        #region Menu
        public void PrintMenuNormal(Menu menu)
        {
            foreach (string menuOption in menu.MenuList)
            {
                if (menu.MenuList.IndexOf(menuOption) == menu.MenuSelect)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(menuOption);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        public void PrintMenuEditStudent(Menu menu, Student student)
        {
            Console.Clear();
            PrintSingleStudent(student);
            Console.WriteLine();
            Console.CursorVisible = false;
            PrintMenuNormal(menu);//printing out the full menu
        }
        #endregion
        #region Students
        public void PrintFoundStudents(IQueryable<Student> foundStudents)
        {
            foreach (Student student in foundStudents)
            {
                PrintSingleStudent(student);
            }
        }
        public void PrintSingleStudent(Student student)
        {
            Console.WriteLine($"Student ID: {student.StudentId}. Name: {student.FirstName} {student.LastName}. Age: {student.StudentAge}. City of Residence: {student.City}.");
        }
        #endregion
    }
}
