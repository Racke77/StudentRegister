using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegister
{
    internal class Student
    {
        public int StudentId { get; set; } //can't set this manually (exception-throw)
        public int StudentAge { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string City { get; set; } = "";

        public Student(int studentAge, string firstName, string lastName, string city)
        {
            StudentAge = studentAge; //remember not to change the words between the "var" and "prop"
            FirstName = firstName;
            LastName = lastName;
            City = city;
        }
    }
}
