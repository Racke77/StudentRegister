using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegister
{
    internal class School
    {
        public StudentDbContext DbStudentContext;
        public Menu Menu;

        public School()
        {
            DbStudentContext = new StudentDbContext();
            Menu = new Menu();
        }
        public void ReceptionDesk()
        {
            while (true)
            {
                int chosenOption = Menu.MenuSelection();
            
                switch (chosenOption)
                {
                    case 0: //Display all students
                        Console.Clear();
                        foreach (var student in DbStudentContext.Students)
                        {
                            PrintStudent(student);
                        }
                        Console.Read();
                        break;
                    case 1: //Create new student
                        AddNewStudent();
                        break;
                    case 2: //Edit existing student
                        var foundStudent = PickStudentToEdit();
                        EditStudent(foundStudent);
                        break;
                    case 3: //Exit program
                        DbStudentContext.SaveChanges();
                        Environment.Exit(1);
                        break;
                }
            }
        }
        public void AddNewStudent()
        {
            //calling the input-methods
            Console.Clear();
            int studentAge = Input.IntInput();
            string firstName = Input.StringInput("first name");
            string lastName = Input.StringInput("last name");
            string city = Input.StringInput("city name");

            //creating the new student and adding them to the database-list
            DbStudentContext.Add(new Student(studentAge, firstName, lastName, city));
            DbStudentContext.SaveChanges();
        }
        public void PrintStudent(Student student)
        {
            Console.WriteLine($"Student ID: {student.StudentId}. Name: {student.FirstName} {student.LastName}. Age: {student.StudentAge}. City of Residence: {student.City}.");
        }
        public List<string> StudentListMenu()
        {
            //create a string-list out of the database -> so that the menu can use it
            List<string> studentList = new List<string>();
            foreach (var student in DbStudentContext.Students)
            {
                studentList.Add($"{student.StudentId}. {student.FirstName} {student.LastName}. {student.StudentAge}. {student.City}.");
            }
            //update the menu and send the list back
            Menu.MenuUpdate(studentList);
            return studentList;
        }
        public Student PickStudentToEdit()
        {
            //create and use the menu
            List<string> studentList = StudentListMenu();
            int studentId = Menu.MenuSelection() +1; //database-index starts at 1 instead of 0
            return FindStudentById(studentId);
        }
        public void EditStudent(Student foundStudent)
        {
            int index = WhatToEditMenu();
            switch (index)
            {
                case 0: //Change ID
                    foundStudent.StudentAge = Input.IntInput();
                    break;
                case 1: //Change First Name
                    foundStudent.FirstName = Input.StringInput("first name");
                    break;
                case 2: //Change Last Name
                    foundStudent.LastName = Input.StringInput("last name");
                    break;
                case 3: //Change City
                    foundStudent.City = Input.StringInput("city name");
                    break;
                case 4: //Delete student
                    DbStudentContext.Students.Remove(foundStudent);
                    break;
                case 5: //Return without doing anything
                    break;
            }
            Menu.StartingMenu();
        }
        public int WhatToEditMenu()
        {
            List<string> editOptions = new List<string>() {
                "Change Age", "Change first name", "Change last name",
                "Change city of residence", "Delete student", "Cancel" };
            Menu.MenuUpdate(editOptions);
            return Menu.MenuSelection();
        }

        public void FindStudentByFullName(string firstName, string lastName)
        {
            var foundStudent = DbStudentContext.Students.Where(s => s.FirstName == firstName && s.LastName == lastName).FirstOrDefault<Student>();
        }
        public Student FindStudentById(int studentId)
        {
            var foundStudent = DbStudentContext.Students.Where(s => s.StudentId == studentId).FirstOrDefault<Student>();
            return foundStudent;
        }
    }
}
