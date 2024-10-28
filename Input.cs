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
        public static int IntInput()
        {
            while (true)
            {
                Console.WriteLine("Please input the student's Age:");
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

                if (inputString != null)
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
    }
}
