

using CoffeeAppWebRole.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CoffeeAppWebRole.DAO
{
    public class TableStorageContextCourses : TableServiceContext
    {
        private const string CoursesTableName = "Courses";

        private static CloudStorageAccount CloudStorageAccount()
        {
            // Retrieve the storage account from the connection string.

            CloudStorageAccount storageAccount = Microsoft.WindowsAzure.CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("TableStorgeConnectionString"));
            return storageAccount;
        }

        public static void CreateTableIfNotExist()
        {
            CloudTableClient tableClient = CloudStorageAccount().CreateCloudTableClient();

            tableClient.CreateTableIfNotExist(CoursesTableName);
        }

        public static Boolean IsTableExisted()
        {
            CloudTableClient tableClient = CloudStorageAccount().CreateCloudTableClient();
            return tableClient.DoesTableExist(CoursesTableName);
        }

        public TableStorageContextCourses () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public IQueryable<Course> Courses
        {
            get { return this.CreateQuery<Course>(CoursesTableName); }
        }

        public void AddCourse(Course course)
        {
            AddObject(CoursesTableName, course);
            SaveChangesWithRetries();
        }

        public void UpdateCourse(Course course)
        {
            UpdateObject(course);
            SaveChangesWithRetries();
        }

        public Course GetCourseById(string id)
        {
            var query = from course in this.CreateQuery<Course>(CoursesTableName)
                        where course.PartitionKey == "Cafe" && course.RowKey == id
                        select course;
            Course cs = null;
            foreach (var c in query)
            {
                cs = c;
            }
            return cs;
        }

        public void ListCourses()
        {
            var query = from course in this.CreateQuery<Course>(CoursesTableName)
                        where course.PartitionKey == "Cafe"
                        select course;
            
            foreach (var c in query)
            {
                Debug.WriteLine("c: {0} {1} {2}", c.RowKey, c.Name, c.Instructor);
            }

        }
    }
}