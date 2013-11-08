using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataPending
    {
        private string B_PENDING = Pending.StatusB.Pending.ToString();
        private string B_IGNORED = Pending.StatusB.Ignored.ToString();

        private static ActionDataPending Instance;

        private static TableStorageContextPendings Context = new TableStorageContextPendings();

        protected ActionDataPending() {
        }

        public static ActionDataPending GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataPending();
            }
            return Instance;
        }

        public void AddPending(string memberid, string requestmemberid)
        {
            var pending = new Pending()
            {
                AMemberID = memberid,
                BMemberID = requestmemberid,
                BStatus = B_PENDING
            };
            Context.AddPending(pending); 
        }

        public void DeletePending(string memberID1, string memberID2)
        {
            Pending pending = Context.GetPendingByState(memberID1, memberID2, B_PENDING);
            if (pending != null)
            {
                Context.DeletePending(pending);
            }
            pending = Context.GetPendingByState(memberID2, memberID1, B_PENDING);
            if (pending != null)
            {
                Context.DeletePending(pending);
            }
        }

        public int GetARequestedPendingCount(string userid)
        {
            IQueryable<Pending> i = GetTargetedMembersB_RequestedPending(userid);
            if (i != null)
                return i.ToList().Count();
            return 0;
        }

        public IQueryable<Pending> GetTargetedMembersA_RequestedPending(string userid)
        {
             return Context.GetTargetedMembersA(userid, B_PENDING);
        }

        public IQueryable<Pending> GetTargetedMembersB_RequestedPending(string userid)
        {
            return Context.GetTargetedMembersB(userid, B_PENDING);
        }

        public void CreateTable()
        {
            TableStorageContextPendings.CreateTableIfNotExist();


        }
    }
}