

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
    public class TableStorageContextMediaResources : TableServiceContext
    {
        private const string MediaResourcesTableName = "MediaResources";

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
            tableClient.CreateTableIfNotExist(MediaResourcesTableName);
        }

        public static Boolean IsTableExisted()
        {
            CloudTableClient tableClient = CloudStorageAccount().CreateCloudTableClient();
            return tableClient.DoesTableExist(MediaResourcesTableName);
        }

        public TableStorageContextMediaResources () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public IQueryable<MediaResource> MediaResources
        {
            get { return this.CreateQuery<MediaResource>(MediaResourcesTableName); }
        }

        public void AddMediaResource(MediaResource mediaResource)
        {
            AddObject(MediaResourcesTableName, mediaResource);
            SaveChangesWithRetries();
        }

        public void UpdateMediaResource(MediaResource mediaResource)
        {
            UpdateObject(mediaResource);
            SaveChangesWithRetries();
        }

        public MediaResource GetMediaResourceById(string id)
        {
            var query = from mediaResource in this.CreateQuery<MediaResource>(MediaResourcesTableName)
                        where mediaResource.PartitionKey == "Cafe" && mediaResource.MediaID == id
                        select mediaResource;
            MediaResource cs = null;
            foreach (var c in query)
            {
                cs = c;
            }
            return cs;
        }
    }
}