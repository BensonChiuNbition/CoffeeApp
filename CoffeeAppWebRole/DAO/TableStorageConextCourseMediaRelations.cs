

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
    public class TableStorageContextCourseMediaRelations : TableServiceContext
    {
        private const string CourseMediaRelationsTableName = "CourseMediaRelations";

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

            tableClient.CreateTableIfNotExist(CourseMediaRelationsTableName);
        }

        public static Boolean IsTableExisted()
        {
            CloudTableClient tableClient = CloudStorageAccount().CreateCloudTableClient();
            return tableClient.DoesTableExist(CourseMediaRelationsTableName);
        }

        public TableStorageContextCourseMediaRelations () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public IQueryable<CourseMediaRelation> CourseMediaRelations
        {
            get { return this.CreateQuery<CourseMediaRelation>(CourseMediaRelationsTableName); }
        }

        public void AddCourseMediaRelation(CourseMediaRelation courseMediaRelation)
        {
            AddObject(CourseMediaRelationsTableName, courseMediaRelation);
            SaveChangesWithRetries();
        }

        public void UpdateCourseMediaRelation(CourseMediaRelation courseMediaRelation)
        {
            UpdateObject(courseMediaRelation);
            SaveChangesWithRetries();
        }

        public CourseMediaRelation GetCourseMediaRelationByCourseIdAndMediaId(string courseId, string mediaId)
        {
            var query = from courseMediaRelation in this.CreateQuery<CourseMediaRelation>(CourseMediaRelationsTableName)
                        where courseMediaRelation.PartitionKey == "Cafe"
                        && courseMediaRelation.CourseID == courseId && courseMediaRelation.MediaID == mediaId
                        select courseMediaRelation;
            CourseMediaRelation e = null;
            foreach (var q in query)
            {
                e = q;
            }
            return e;
        }

        public IQueryable<CourseMediaRelation> GetCourseMediaRelationsByCourseId(string courseId)
        {
            var query = from courseMediaRelation in this.CreateQuery<CourseMediaRelation>(CourseMediaRelationsTableName)
                        where courseMediaRelation.PartitionKey == "Cafe" && courseMediaRelation.CourseID == courseId
                        select courseMediaRelation;
            return query;
        }

        public IQueryable<CourseMediaRelation> GetCourseMediaRelationsByMediaId(string mediaId)
        {
            var query = from courseMediaRelation in this.CreateQuery<CourseMediaRelation>(CourseMediaRelationsTableName)
                        where courseMediaRelation.PartitionKey == "Cafe" && courseMediaRelation.MediaID == mediaId
                        select courseMediaRelation;
            return query;
        }
    }
}