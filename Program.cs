using LAB_3_SQL_ORM.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace LAB_3_SQL_ORM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true) // förklaring till meny
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("*******************************************************************");
                Console.WriteLine("Choose a function:");
                Console.WriteLine("1. Get personnel");
                Console.WriteLine("2. Get all students");
                Console.WriteLine("3. Get all students in a specific class");
                Console.WriteLine("4. Get all grades set in the last month");
                Console.WriteLine("5. Get a list of all courses and average grades");
                Console.WriteLine("6. Add new students");
                Console.WriteLine("7. Add new personnel");
                Console.WriteLine("8. Exit");
                Console.ForegroundColor = ConsoleColor.White;

                string choice = Console.ReadLine();

                switch (choice) //användarens meny
                {
                    
                    case "1":
                        GetPersonnel();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        GetAllStudents();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        GetStudentsInClass();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "4":
                        GetGradesLastMonth();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "5":
                        GetCourseAverageGrades();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "6":
                        AddNewStudent();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "7":
                        AddNewPersonnel();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "8":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }



        static void GetPersonnel() // här kan användaren se alla anstälda eller i kategorier
        {
            using (var context = new TheSchoolContext())
            {
                Console.WriteLine("here you can choose which personel you want their name");
                Console.WriteLine("to see all teacher tab 1");
                Console.WriteLine("to see all Admin tab 2");
                Console.WriteLine("to see the principle tab 3");
                Console.WriteLine("to see the janitors tab 4");
                Console.WriteLine("to see all personel tab 5");
                Console.WriteLine("press key after seeing the info to get back to menu");

                int choosePersonnel = int.Parse(Console.ReadLine());

                var filteredPersonnel = context.People
                    .OrderBy(p => p.PersonId)
                    .Where(s =>
                                (choosePersonnel == 1 && s.Position == "Teacher") ||
                                (choosePersonnel == 2 && s.Position == "Admin") ||
                                (choosePersonnel == 3 && s.Position == "Pricipal") ||
                                (choosePersonnel == 4 && s.Position == "Janitor") ||
                                (choosePersonnel == 5 && (s.Position == "Teacher" || s.Position == "Admin")
                                || s.Position == "Janitor" || s.Position == "Pricipal"))
                    .OrderBy(s => s.FirstName)
                    .ThenBy(s => s.LastName)
                    .ToList();

                foreach (var personnel in filteredPersonnel)
                {
                    Console.WriteLine($"Id:{personnel.PersonId} - Position: {personnel.Position} - Name:{personnel.FirstName} {personnel.LastName} ");
                }
            }
        }
        static void GetAllStudents() // här kan användaren se elever soreterade efter förnamn eller efternamn
        {
            using (var context1 = new TheSchoolContext())
            {
                Console.WriteLine("too show all students in order by Lastname tab 1");
                Console.WriteLine("too show all students in order by Firstname tab anything");
                Console.WriteLine("press key after seeing the info to get back to menu");
                int choosenOrder = int.Parse(Console.ReadLine());
                if (choosenOrder == 1)
                {
                    var students = context1.Students
                           .Include(s => s.FkPerson)
                           .OrderBy(s => s.FkPerson.LastName)
                           .ThenBy(s => s.FkPerson.FirstName)
                           .ToList();
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.FkPerson.FirstName} {student.FkPerson.LastName} - {student.FkPerson.Position}:");
                    }
                }
                else
                {
                    var students = context1.Students
                          .Include(s => s.FkPerson)
                          .OrderBy(s => s.FkPerson.FirstName)
                          .ThenBy(s => s.FkPerson.LastName)
                          .ThenBy(s => s.FkPerson.Position)
                          .ToList();
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.FkPerson.FirstName} {student.FkPerson.LastName} - {student.FkPerson.Position}:");
                    }
                }
            }
        }

        static void GetStudentsInClass() // här kan användaren se elever i en viss klass
        {
            using (var context = new TheSchoolContext())
            {
                var classes = context.Students.Select(s => s.Class).Distinct().ToList();
                Console.WriteLine("Choose a class:");
                foreach (var className in classes)
                {
                    Console.WriteLine(className);
                }

                string selectedClass = Console.ReadLine();

                var studentsInClass = context.Students
                    .Where(s => s.Class == selectedClass)
                    .OrderBy(s => s.StudentId)
                    .Include(s => s.FkPerson)
                    .OrderBy(s => s.FkPerson.FirstName)
                    .ThenBy(s => s.FkPerson.LastName)
                    .ToList();

                foreach (var student in studentsInClass)
                {
                    Console.WriteLine($"{student.FkPerson.FirstName} {student.FkPerson.LastName} - Class: {student.Class}");
                }
            }
        }

        static void GetGradesLastMonth() // här kan användaren se betyg från sista månad
        {
            using (var context = new TheSchoolContext())
            {
                var gradesLastMonth = context.Grades
                        .Include(g => g.FkStudent.FkPerson)
                        .Include(g => g.FkCourse)
                        .Where(g => g.GradeDate >= DateTime.Now.AddMonths(-1))
                        .OrderBy(g => g.GradeDate)
                        .ToList();

                foreach (var grade in gradesLastMonth)
                {
                    Console.WriteLine($"{grade.FkStudent.FkPerson.FirstName} {grade.FkStudent.FkPerson.LastName} - Course: {grade.FkCourse.CourseName}, Grade: {grade.Grade1}");
                }
            }
        }

        static void GetCourseAverageGrades() // här kan användaren se snittbetyg som eleverna fått på kursen samt högsta och lägsta
        {
            using (var context = new TheSchoolContext())
            {
                Console.WriteLine("the highest grade is 3 and lowest is 0");
                Console.WriteLine("press key after seeing the info to get back to menu");
                var courses = context.Courses.ToList();
                foreach (var course in courses)
                {
                    var gradesForCourse = context.Grades.Where(g => g.FkCourseId == course.CourseId).ToList();

                    if (gradesForCourse.Any())
                    {
                        var averageGrade = gradesForCourse
                            .Where(g => g.Grade1 != null)
                            .Average(g => int.Parse(g.Grade1.ToString()));

                        var maxGrade = gradesForCourse
                            .Where(g => g.Grade1 != null)
                            .Max(g => int.Parse(g.Grade1.ToString()));

                        var minGrade = gradesForCourse
                            .Where(g => g.Grade1 != null)
                            .Min(g => int.Parse(g.Grade1.ToString()));

                        Console.WriteLine($"Course: {course.CourseName}, Average Grade: {averageGrade}, Max: {maxGrade}, Min: {minGrade}");
                    }
                    else
                    {
                        Console.WriteLine($"Course: {course.CourseName} - No grades available");
                    }
                }
            }
        }

            static void AddNewStudent() // här addas ny elev, användaren bestämmer ID, namn osv
            {
                using (var context = new TheSchoolContext())
                {
                    Console.WriteLine("Enter new student details:");

                    Console.Write("First Name: ");
                    string firstName = Console.ReadLine();

                    Console.Write("Last Name: ");
                    string lastName = Console.ReadLine();

                    Console.Write("Class: ");
                    string studentClass = Console.ReadLine();

                    Console.WriteLine("ID: ");
                    int StudentId = int.Parse(Console.ReadLine());
                    

                    var newStudent = new Student
                    {
                        FkPerson = new Person
                        {
                        
                            FirstName = firstName,
                            LastName = lastName,
                            PersonId = StudentId
                        },
                        Class = studentClass
                    };

                    context.Students.Add(newStudent);
                    context.SaveChanges();

                    Console.WriteLine("New student added successfully!");
               
                }
            }

        static void AddNewPersonnel() // här addas ny personal, användar bestämmer id, namn osv
        {
            using (var context = new TheSchoolContext())
            {
                Console.WriteLine("Enter new personnel details:");
                Console.WriteLine("ID: ");
                int PersonId = int.Parse(Console.ReadLine());

                Console.Write("First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Position: ");
                string position = Console.ReadLine();

                var newPersonel = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Position = position,
                    PersonId = PersonId
                };

                context.People.Add(newPersonel);
                context.SaveChanges();

                Console.WriteLine("New personnel added successfully!");

            }

        }
    }
}
