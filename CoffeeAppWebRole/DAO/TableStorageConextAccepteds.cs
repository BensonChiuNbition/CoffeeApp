

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
    public class TableStorageContextAccepteds : TableServiceContext
    {
        private const string AcceptedsTableName = "Accepteds";

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

            tableClient.CreateTableIfNotExist(AcceptedsTableName);
        }

        public TableStorageContextAccepteds () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public IQueryable<Accepted> Accepteds
        {
            get { return this.CreateQuery<Accepted>(AcceptedsTableName); }
        }

        public void AddAccepted(Accepted friendship)
        {
            AddObject(AcceptedsTableName, friendship);
            SaveChangesWithRetries();
        }

        public Accepted GetAccepted(string id1, string id2)
        {
            var query = from accepted in this.CreateQuery<Accepted>(AcceptedsTableName)
                        where accepted.PartitionKey == "Cafe"
                        && (accepted.MemberID1 == id1 && accepted.MemberID2 == id2)
                        select accepted;
            Accepted a = null;
            foreach (var q in query)
            {
                a = q;
            }
            return a;
        }

        public IQueryable<Accepted> GetAccepteds(string id)
        {
            var query = from accepted in this.CreateQuery<Accepted>(AcceptedsTableName)
                        where accepted.PartitionKey == "Cafe"
                        && (accepted.MemberID1 == id || accepted.MemberID2 == id)
                        select accepted;
            return query;
        }

        public void ListAccepteds()
        {
            var query = from accepted in this.CreateQuery<Accepted>(AcceptedsTableName)
                        where accepted.PartitionKey == "Cafe"
                        select accepted;
            
            foreach (var n in query)
            {
                Debug.WriteLine("c: {0} {1} {2}", n.RowKey, n.MemberID1, n.MemberID2);
            }

        }
    }
}