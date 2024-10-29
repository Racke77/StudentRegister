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
        #region Main-program
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
        #endregion
        #region Add new student
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
        #endregion
        #region Printing
        public void PrintStudent(Student student)
        {
            Console.WriteLine($"Student ID: {student.StudentId}. Name: {student.FirstName} {student.LastName}. Age: {student.StudentAge}. City of Residence: {student.City}.");
        }
        public void PrintFoundStudents()
        {
            foreach (Student student in FoundStudents)
            {
                PrintStudent(student);
            }
        }
        #endregion
        #region Making a selectable list of students
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
        public List<int> StudentIdMenu()
        {
            List<int> studentIds = new List<int>();
            foreach (var student in DbStudentContext.Students)
            {
                studentIds.Add(student.StudentId);
            }
            return studentIds;
        }
        #endregion
        #region Editing a student
        public Student PickStudentToEdit()
        {
            Menu.MenuUpdate(new List<string>() { "Pick from a list", "Find by ID" });
            switch (Menu.MenuSelection())
            {
                case 0:
                    //create a menu of students
                    Menu.MenuUpdate(StudentListMenu());
                    //select the ID-value at menu-selected
                    int studentId = StudentIdMenu()[Menu.MenuSelection()];
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
        #endregion
        #region Find students
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
                case 4://age
                    break;
                case 5: //search by ID
                    var foundStudent = FindStudentById(Input.IntInput("ID"));
                    break;
            }
        }
        #region Finding a single student
        public Student FindStudentById(int studentId)
        {
            var foundStudent = DbStudentContext.Students.Where(s => s.StudentId == studentId).FirstOrDefault<Student>();
            return foundStudent;
        }
        #endregion
        #region For the finding-menu
        public List<string> FindStudentsOptions()
        {
            List<string> findStudents = new List<string>() {
                "Search by full name",
                "Search by first name",
                "Search by last name",
                "Search by city",
                "Search by age",
                "Search by ID"
            };
            return findStudents;
        }
        #endregion
        #region Search by
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
        public void FindStudentByAge(int age)
        {
            FoundStudents = DbStudentContext.Students.Where(s => s.StudentAge == age);
        }
        #endregion
        #endregion
    }
}
