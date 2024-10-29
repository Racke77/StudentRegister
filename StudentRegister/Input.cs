using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegister
{
    internal class Input
    {
        public static int IntInput(string desiredInput)
        {
            while (true)
            {
                Console.WriteLine($"Please input the student's {desiredInput}:");
                Console.CursorVisible = true;
                try
                {
                    int studentId = Convert.ToInt32(Console.ReadLine());
                    if (studentId < 0)
                    {
                        Console.WriteLine("You can't use negative numbers");
                    }
                    else
                    {
                        return studentId;
                    }
                }
                catch
                {
                    Console.CursorVisible = false;
                    Console.WriteLine("Please only input numbers.");
                }
                Console.Clear();
            }
        }
        public static string StringInput(string requestedString)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"Please input {requestedString}.");
                Console.CursorVisible = true;
                string? inputString = Console.ReadLine();

                if (inputString != null && inputString != "")
                {
                    bool containsInt = inputString.Any(char.IsDigit);
                    if (!containsInt)
                    {
                        return inputString;
                    }
                    else
                    {
                        Console.WriteLine("Please do not include numbers.");
                    }
                }
                else
                {
                    Console.WriteLine($"You need to include a {requestedString}.");
                }
            }
        }
        #region string-types
        public static string CityName()
        {
            return "city";
        }
        public static string FirstName()
        {
            return "first name";
        }
        public static string LastName()
        {
            return "last name";
        }
        public static string Age()
        {
            return "age";
        }
        #endregion
    }
}
