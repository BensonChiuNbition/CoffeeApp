

using CoffeeAppWebRole.ActionData;
using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CoffeeAppWebRole.Controllers
{
    public class HomeController : Controller
    {
        private ActionDataMember ADM = ActionDataMember.GetInstance();
        private ActionDataCourse ADC = ActionDataCourse.GetInstance();
        private ActionDataEnrollment ADE = ActionDataEnrollment.GetInstance();
        private ActionDataPending ADP = ActionDataPending.GetInstance();
        private ActionDataAccepted ADA = ActionDataAccepted.GetInstance();

        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return RedirectToAction("Login", "Account");
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            ADM.CreateTable();
            ADC.CreateTable();
            ADE.CreateTable();
            ADP.CreateTable();
            ADA.CreateTable();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";


            return View();
        }

        public ActionResult CourseList(string userid, string dac)
        {
            ViewBag.Message = "Your course list page!";

            if (userid != null && dac != null)
            {
                ADM.UpdateMember_DataAccessControl(userid, dac);
            }


            Dictionary<string,string> d = new Dictionary<string, string>();
            IQueryable<Enrollment> enrollments;
            if (userid != null)
            {
                enrollments = ADE.GetEnrollmentsByMemberId(userid);
                foreach (var q in enrollments)
                {
                    d.Add(q.CourseID, q.EnrollStatus);
                }
            }


            int requestedPendingCount = 0;
            if (userid != null)
            {
                requestedPendingCount = ADP.GetARequestedPendingCount(userid);
            }
            ViewBag.Courses = ADC.GetCourses();
            ViewBag.EnrollmentInfos = d;
            ViewBag.RequestedPendingCount = requestedPendingCount.ToString();


            return View();
        }

        public ActionResult CourseInfo(string userid, string courseCode)
        {
            ViewBag.Message = "Your course info page: " + courseCode;

            ViewBag.EnrollmentInfo = "";
            Enrollment en = ADE.GetEnrollmentByMemberIdAndCourseId(userid, courseCode);
            if (en != null)
            {
                ViewBag.EnrollmentInfo = en.EnrollStatus;
            }

            Course c = ADC.GetCourseById(courseCode);
            ViewBag.CourseID = c.RowKey;
            ViewBag.Name = c.Name;
            ViewBag.Description = c.Description;
            ViewBag.DateStart = c.DateStart;
            ViewBag.DateEnd = c.DateEnd;
            ViewBag.NumOfStudent = c.NumOfStudent;
            ViewBag.CourseDateTime = c.CourseDateTime;
            ViewBag.Instructor = c.Instructor;

            return View();
        }

        public ActionResult CourseMediaResource()
        {
            ViewBag.Message = "Your course media resource page.";

            return View();
        }

        public ActionResult CourseClassmateList(string userid, string courseid, string requestmemberid)
        {
            ViewBag.Message = "Your course classmate list page.";

            //Find classmates
            List<Member> members = new List<Member>();
            IQueryable<Enrollment> enrollments;
            Member m;
            if (courseid != null)
            {  
                enrollments = ADE.GetEnrollmentsByCourseId(courseid);
                foreach (var q in enrollments)
                {
                    if (!userid.Equals(q.MemberID)){
                        m = ADM.GetMemberById(q.MemberID);
                        members.Add(m);               
                    }
                }
            }
            

            //Add friendship
            if (userid != null && requestmemberid != null)
            {
                ADP.AddPending(userid, requestmemberid);  
            }


            //Search for friends that are already friend-requested
            Hashtable t = new Hashtable();
            Hashtable t2 = new Hashtable();
            if (userid != null)
            {
                IQueryable<Pending> friendsRequestedPending = ADP.GetTargetedMembersA_RequestedPending(userid);
                foreach (var q in friendsRequestedPending)
                {
                    if (!t.ContainsKey(q.BMemberID))
                    {
                        t.Add(q.BMemberID, null);
                    }
                }

                IQueryable<Accepted>  accepteds = ADA.GetAccepteds(userid);
                foreach (var q in accepteds)
                {
                    if (userid.Equals(q.MemberID1))
                    {
                        m = ADM.GetMemberById(q.MemberID2);
                    }
                    else
                    {
                        m = ADM.GetMemberById(q.MemberID1);
                    }

                    if (!t2.ContainsKey(m.RowKey))
                    {
                        t2.Add(m.RowKey, null);
                    }
                }
            }

            ViewBag.Members = members;
            ViewBag.FriendsRequestedPendingHashTable = t;
            ViewBag.FriendsAcceptedHashTable = t2;
            ViewBag.CourseID = courseid;

            return View();
        }

        public ActionResult FriendMadeList(string userid, string acceptmemberid)
        {
            ViewBag.Message = "Your course friend list page.";

            //Find classmates
            List<Member> members = new List<Member>();
            IQueryable<Pending> friendships;
            IQueryable<Accepted> accepteds;
            Member m;
            if (userid != null)
            {
                friendships = ADP.GetTargetedMembersA_RequestedPending(userid);
                foreach (var q in friendships)
                {
                    m = ADM.GetMemberById(q.BMemberID);
                    members.Add(m);
                }

                friendships = ADP.GetTargetedMembersB_RequestedPending(userid);
                foreach (var q in friendships)
                {
                    m = ADM.GetMemberById(q.AMemberID);
                    members.Add(m);
                }

                accepteds = ADA.GetAccepteds(userid);
                foreach (var q in accepteds)
                {
                    if (userid.Equals(q.MemberID1))
                    {
                        m = ADM.GetMemberById(q.MemberID2);
                    }
                    else
                    {
                        m = ADM.GetMemberById(q.MemberID1);
                    }

                    members.Add(m);
                }
            }

            
            //Update friendship
            if (userid != null && acceptmemberid != null)
            {
                //ADF.AcceptFriendship(userid, acceptmemberid);
                ADA.AddAccepted(userid, acceptmemberid);
                ADP.DeletePending(userid, acceptmemberid);
            }

            
            //Search for friends that are already friended
            Hashtable t = new Hashtable();
            Hashtable t2 = new Hashtable();
            if (userid != null)
            {
                IQueryable<Accepted> friendsFriendedAccepted = ADA.GetAccepteds(userid);
                foreach (var q in friendsFriendedAccepted)
                {
                    if (userid.Equals(q.MemberID1))
                    {
                        m = ADM.GetMemberById(q.MemberID2);
                    }
                    else
                    {
                        m = ADM.GetMemberById(q.MemberID1);
                    }

                    if (!t.ContainsKey(m.RowKey))
                    {
                        t.Add(m.RowKey, null);
                    }
                }

                IQueryable<Pending> friendsRequestedPending = ADP.GetTargetedMembersA_RequestedPending(userid);
                foreach (var q in friendsRequestedPending)
                {
                    if (!t2.ContainsKey(q.BMemberID))
                    {
                        t2.Add(q.BMemberID, null);
                    }
                }
            }
            
            ViewBag.Members = members;
            ViewBag.FriendsFriendedAcceptedHashTable = t;
            ViewBag.FriendsRequestedPendingHashTable = t2;


            return View();
        }

        public ActionResult Profile(String userid)
        {

            ViewBag.Message = "Your profile page.";

            Member mb = ADM.GetMemberById(userid);

            Dictionary<string, string> mapMember = new Dictionary<string, string>();
            mapMember.Add("NameEN", mb.NameEN);
            mapMember.Add("NameCH", mb.NameCH);
            mapMember.Add("Gender", mb.Gender);
            mapMember.Add("MailAddress", mb.MailAddress);
            mapMember.Add("WorkAddress", mb.WorkAddress);
            mapMember.Add("EMail", mb.EMail);
            mapMember.Add("DOB", mb.DOB);
            mapMember.Add("Occupation", mb.Occupation);
            mapMember.Add("Phone", mb.Phone);
            mapMember.Add("ProfilePic", "");
            
            Dictionary<string, int> dataAccessControlMap =
                JsonConvert.DeserializeObject<Dictionary<string, int>>(mb.DataAccessControl);

            ViewBag.TargetMember = mb;
            ViewBag.MapMember = mapMember;
            ViewBag.DataAccessControlMap = dataAccessControlMap;

            return View();
        }
    }
}
