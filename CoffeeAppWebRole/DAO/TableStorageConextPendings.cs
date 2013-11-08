

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
    public class TableStorageContextPendings : TableServiceContext
    {
        private const string PendingsTableName = "Pendings";

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

            tableClient.CreateTableIfNotExist(PendingsTableName);
        }

        public TableStorageContextPendings () : base(CloudStorageAccount().TableEndpoint.ToString(), CloudStorageAccount().Credentials){
            
        }

        public IQueryable<Pending> Pendings
        {
            get { return this.CreateQuery<Pending>(PendingsTableName); }
        }

        public void AddPending(Pending pending)
        {
            AddObject(PendingsTableName, pending);
            SaveChangesWithRetries();
        }

        public void UpdatePending(Pending pending)
        {
            UpdateObject(pending);
            SaveChangesWithRetries();
        }

        public void DeletePending(Pending pending)
        {
            DeleteObject(pending);
            SaveChangesWithRetries();
        }

        public Pending GetPendingByState(string aID, string bID, string bStatus)
        {
            var query = from pending in this.CreateQuery<Pending>(PendingsTableName)
                        where pending.PartitionKey == "Cafe"
                        && pending.AMemberID == aID
                        && pending.BMemberID == bID
                        && pending.BStatus == bStatus
                        select pending;
            Pending f = null;
            foreach (var q in query)
            {
                f = q;
            }
            return f;
        }

        public IQueryable<Pending> GetTargetedMembersA(string id, string bStatus)
        {
            var query = from pending in this.CreateQuery<Pending>(PendingsTableName)
                        where pending.PartitionKey == "Cafe"
                        && pending.AMemberID == id
                        && pending.BStatus == bStatus
                        select pending;
            return query;
        }

        public IQueryable<Pending> GetTargetedMembersB(string id, string bStatus)
        {
            var query = from pending in this.CreateQuery<Pending>(PendingsTableName)
                        where pending.PartitionKey == "Cafe"
                        && pending.BMemberID == id
                        && pending.BStatus == bStatus
                        select pending;
            return query;
        }

        public void ListPendings()
        {
            var query = from pending in this.CreateQuery<Pending>(PendingsTableName)
                        where pending.PartitionKey == "Cafe"
                        select pending;
            
            foreach (var n in query)
            {
                Debug.WriteLine("c: {0} {1} {2} {3}", n.RowKey, n.AMemberID, n.BMemberID, n.BStatus);
            }

        }
    }
}