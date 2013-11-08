

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
    public class TableStorageContextEnrollments : TableServiceContext
    {
        private const string EnrollmentsTableName = "Enrollments";

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

            tableClient.CreateTableIfNotExist(EnrollmentsTableName);
        }

        public TableStorageContextEnrollments () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public IQueryable<Enrollment> Enrollments
        {
            get { return this.CreateQuery<Enrollment>(EnrollmentsTableName); }
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            AddObject(EnrollmentsTableName, enrollment);
            SaveChangesWithRetries();
        }

        public void UpdateEnrollment(Enrollment enrollment)
        {
            UpdateObject(enrollment);
            SaveChangesWithRetries();
        }

        public Enrollment GetEnrollmentByMemberIdAndCourseId(string memberId, string courseId)
        {
            var query = from enrollment in this.CreateQuery<Enrollment>(EnrollmentsTableName)
                        where enrollment.PartitionKey == "Cafe"
                        && enrollment.MemberID == memberId && enrollment.CourseID == courseId
                        select enrollment;
            Enrollment e = null;
            foreach (var q in query)
            {
                e = q;
            }
            return e;
        }

        public IQueryable<Enrollment> GetEnrollmentsByMemberId(string memberId)
        {
            var query = from enrollment in this.CreateQuery<Enrollment>(EnrollmentsTableName)
                        where enrollment.PartitionKey == "Cafe" && enrollment.MemberID == memberId
                        select enrollment;
            return query;
        }

        public IQueryable<Enrollment> GetEnrollmentsByCourseId(string courseId)
        {
            var query = from enrollment in this.CreateQuery<Enrollment>(EnrollmentsTableName)
                        where enrollment.PartitionKey == "Cafe" && enrollment.CourseID == courseId
                        select enrollment;
            return query;
        }

        public void ListEnrollments()
        {
            var query = from enrollment in this.CreateQuery<Enrollment>(EnrollmentsTableName)
                        where enrollment.PartitionKey == "Cafe"
                        select enrollment;
            
            foreach (var n in query)
            {
                Debug.WriteLine("c: {0} {1} {2}", n.RowKey, n.CourseID, n.MemberID, n.EnrollStatus);
            }

        }
    }
}