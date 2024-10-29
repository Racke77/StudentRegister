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
        public IQueryable<Student> FoundStudents;

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
                    case 3: //Find list
                        FindStudentList();
                        break;
                    case 4: //Exit program
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
            int studentAge = Input.IntInput(Input.Age());
            string firstName = Input.StringInput(Input.FirstName());
            string lastName = Input.StringInput(Input.LastName());
            string city = Input.StringInput(Input.CityName());

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
            Menu.MenuUpdate(new List<string>() { "Pick from a list", "Find by ID" });
            switch (Menu.MenuSelection())
            {
                case 0:
                    //create a menu of students
                    List<string> studentList = StudentListMenu();
                    Menu.MenuUpdate(studentList);

                    int studentId = Menu.MenuSelection() + 1; //database-index starts at 1 instead of 0
                                                              //risky -> if the database has ID-gaps where it suddenly skips to 100 -> won't work
                    return FindStudentById(studentId);
                case 1:
                    studentId = Input.IntInput("ID");
                    return FindStudentById(studentId);
                default: 
                    return null;
            }
        }
        public void EditStudent(Student foundStudent)
        {
            int index = WhatToEditMenu();
            switch (index)
            {
                case 0: //Change ID
                    foundStudent.StudentAge = Input.IntInput(Input.Age());
                    break;
                case 1: //Change First Name
                    foundStudent.FirstName = Input.StringInput(Input.FirstName());
                    break;
                case 2: //Change Last Name
                    foundStudent.LastName = Input.StringInput(Input.LastName());
                    break;
                case 3: //Change City
                    foundStudent.City = Input.StringInput(Input.CityName());
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
        public void FindStudentList()
        {
            Menu.MenuUpdate(FindStudentsOptions());
            switch (Menu.MenuSelection())
            {
                case 0://full name
                    string firstName = Input.StringInput(Input.FirstName());
                    string lastName = Input.StringInput(Input.LastName());
                    FindStudentByFullName(firstName, lastName);
                    PrintFoundStudents();
                    break;
                case 1://first name
                    firstName = Input.StringInput(Input.FirstName());
                    FindStudentByFirstName(firstName);
                    PrintFoundStudents();
                    break;
                case 2://last name
                    lastName = Input.StringInput(Input.LastName());
                    FindStudentByLastName(lastName);
                    PrintFoundStudents();
                    break;
                case 3://city
                    string city = Input.StringInput(Input.CityName());
                    FindStudentByCity(city);
                    PrintFoundStudents();
                    break;
                case 4: //search by ID
                    var foundStudent = FindStudentById(Input.IntInput("ID"));
                    break;
            }
        }
        public List<string> FindStudentsOptions()
        {
            List<string> findStudents = new List<string>() {
                "Search by full name",
                "Search by first name",
                "Search by last name",
                "Search by city"
            };
            return findStudents;
        }
        public void PrintFoundStudents()
        {
            foreach (Student student in FoundStudents)
            {
                PrintStudent(student);
            }
        }
        public void FindStudentByFirstName(string firstName)
        {
            FoundStudents = DbStudentContext.Students.Where(s => s.FirstName == firstName);
        }
        public void FindStudentByLastName(string lastName)
        {
            FoundStudents = DbStudentContext.Students.Where(s => s.LastName == lastName);
        }
        public void FindStudentByFullName(string firstName, string lastName)
        {
            FoundStudents = DbStudentContext.Students.Where(s => s.FirstName == firstName && s.LastName == lastName);
        }
        public void FindStudentByCity(string city)
        {
            FoundStudents = DbStudentContext.Students.Where(s => s.City == city);
        }
        public Student FindStudentById(int studentId)
        {
            var foundStudent = DbStudentContext.Students.Where(s => s.StudentId == studentId).FirstOrDefault<Student>();
            return foundStudent;
        }
    }
}
