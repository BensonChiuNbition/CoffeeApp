using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataCourse
    {
        private static ActionDataCourse Instance;

        private static TableStorageContextCourses Context = new TableStorageContextCourses();

        protected ActionDataCourse() {
        }

        public static ActionDataCourse GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataCourse();
            }
            return Instance;
        }

        public IQueryable<Course> GetCourses()
        {
            return Context.Courses;
        }

        public Course GetCourseById(string courseCode)
        {
            return Context.GetCourseById(courseCode);
        }

        public void CreateTable()
        {
            if (!TableStorageContextAccepteds.IsTableExisted())
            {
                TableStorageContextCourses.CreateTableIfNotExist();

                var course = new Course()
                {
                    //CourseID = "COFA00",
                    //RowKey = Guid.NewGuid().ToString(),
                    RowKey = "COFA03",
                    Name = "Tasting Session Level 1",
                    Description = "Cupping is one of the coffee tasting techniques used by cuppers to evaluate coffee aroma and the flavor profile of a coffee. To understand the minor differences between coffee growing regions, it is important to taste coffee from around the world side-by-side.",
                    DateStart = "2013/11/20",
                    DateEnd = "2013/12/10",
                    NumOfStudent = "15",
                    CourseDateTime = "W 19:30-21:00, F 20:30-22:00",
                    Instructor = "Wyman"
                };
                var course2 = new Course()
                {
                    //CourseID = "COFA00",
                    //RowKey = Guid.NewGuid().ToString(),
                    RowKey = "COFB07",
                    Name = "Coffee History",
                    Description = "The Ethiopian ancestors of today's Oromo ethnic group, were the first to have recognized the energizing effect of the native coffee plant.",
                    DateStart = "2013/11/10",
                    DateEnd = "2013/11/29",
                    NumOfStudent = "10",
                    CourseDateTime = "T 19:30-21:00",
                    Instructor = "Moses"
                };
                var course3 = new Course()
                {
                    //CourseID = "COFA00",
                    //RowKey = Guid.NewGuid().ToString(),
                    RowKey = "COFB09",
                    Name = "Coffee History 2",
                    Description = "The Ethiopian ancestors of today's Oromo ethnic group, were the first to have recognized the energizing effect of the native coffee plant.",
                    DateStart = "2013/11/10",
                    DateEnd = "2013/11/29",
                    NumOfStudent = "10",
                    CourseDateTime = "T 19:30-21:00",
                    Instructor = "Moses II"
                };
                Context.AddCourse(course);
                Context.AddCourse(course2);
                Context.AddCourse(course3);
            }
        }
    }
}