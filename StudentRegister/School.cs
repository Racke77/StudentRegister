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
                int chosenOption = Menu.MenuSelectionNormal();

                switch (chosenOption)
                {
                    case 0: //Display all students
                        Console.Clear();
                        foreach (var student in DbStudentContext.Students)
                        {
                            Menu.Printer.PrintSingleStudent(student);
                        }
                        Console.ReadLine();
                        break;
                    case 1: //Create new student
                        AddNewStudent();
                        break;
                    case 2: //Edit existing student
                        var foundStudent = PickStudentToEdit();
                        EditStudent(foundStudent);
                        break;
                    case 3: //Find list
                        SearchForAllStudentsByX();
                        Console.ReadLine();
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
        #region Making a selectable list of students
        //Would place this in the Menu, but sending objects is more intense than sending string-lists
        public List<string> StudentListMenu()
        {
            //create a string-list out of the database -> so that the menu can use it
            List<string> studentList = new List<string>();
            foreach (var student in DbStudentContext.Students)
            {
                studentList.Add($"{student.StudentId}. {student.FirstName} {student.LastName}. {student.StudentAge}. {student.City}.");
            }
            return studentList;
        }
        public List<int> StudentIdMenu()
        {
            //create an int-list out of the database -> same order as the string-list (can read ID by selecting the string-list)
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
            Menu.StudentEditHowToPickStudent();
            switch (Menu.MenuSelectionNormal())
            {
                case 0:
                    //create a menu of students
                    Menu.MenuUpdate(StudentListMenu());
                    //create a menu of IDs -> select the ID at index
                    int studentId = StudentIdMenu()[Menu.MenuSelectionNormal()];
                    return FindStudentById(studentId);
                case 1:
                    studentId = Input.IntInput(Input.Id());
                    return FindStudentById(studentId);
            }
            return null;
        }
        public void EditStudent(Student foundStudent)
        {
            int index = WhatToEditMenu(foundStudent);
            switch (index)
            {
                case 0: //Change Age
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
            DbStudentContext.SaveChanges();
            Menu.StartingMenu();
        }
        public int WhatToEditMenu(Student student)
        {
            Menu.WhatToEditAboutStudent();
            return Menu.MenuSelectionEditStudent(student);
        }
        #endregion
        #region Find students
        public void SearchForAllStudentsByX()
        {
            Menu.FindStudentsByXOptions();
            switch (Menu.MenuSelectionNormal())
            {
                case 0://full name
                    string firstName = Input.StringInput(Input.FirstName());
                    string lastName = Input.StringInput(Input.LastName());
                    FindStudentByFullName(firstName, lastName);
                    Menu.Printer.PrintFoundStudents(FoundStudents);
                    break;
                case 1://first name
                    firstName = Input.StringInput(Input.FirstName());
                    FindStudentByFirstName(firstName);
                    Menu.Printer.PrintFoundStudents(FoundStudents);
                    break;
                case 2://last name
                    lastName = Input.StringInput(Input.LastName());
                    FindStudentByLastName(lastName);
                    Menu.Printer.PrintFoundStudents(FoundStudents);
                    break;
                case 3://city
                    string city = Input.StringInput(Input.CityName());
                    FindStudentByCity(city);
                    Menu.Printer.PrintFoundStudents(FoundStudents);
                    break;
                case 4://age
                    int age = Input.IntInput(Input.Age());
                    FindStudentByAge(age);
                    Menu.Printer.PrintFoundStudents(FoundStudents);
                    break;
                case 5: //search by ID
                    var foundStudent = FindStudentById(Input.IntInput(Input.Id()));
                    Menu.Printer.PrintSingleStudent(foundStudent);
                    break;
            }
            Menu.StartingMenu(); //update to starting-menu
        }
        #region Finding a single student
        public Student FindStudentById(int studentId)
        {
            Student? foundStudent = null;
            while (foundStudent==null) //will loop until valid ID is given
            {
                foundStudent = DbStudentContext.Students.Where(s => s.StudentId == studentId).FirstOrDefault<Student>();
                if (foundStudent == null) //if student is STILL null -> wrong ID
                {
                    studentId = Input.IdCatcher();
                }
            }
            return foundStudent;
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
