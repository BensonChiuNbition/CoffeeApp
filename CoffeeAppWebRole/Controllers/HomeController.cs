

using CoffeeAppWebRole.ActionData;
using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using Microsoft.Web.WebPages.OAuth;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using WebMatrix.WebData;

namespace CoffeeAppWebRole.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ActionDataMember ADM = ActionDataMember.GetInstance();
        private ActionDataCourse ADC = ActionDataCourse.GetInstance();
        private ActionDataEnrollment ADE = ActionDataEnrollment.GetInstance();
        private ActionDataPending ADP = ActionDataPending.GetInstance();
        private ActionDataAccepted ADA = ActionDataAccepted.GetInstance();
        private ActionDataMediaResource ADMR = ActionDataMediaResource.GetInstance();
        private ActionDataCourseMediaRelation ADCMR = ActionDataCourseMediaRelation.GetInstance();
        private ActionDataBlob ADB = ActionDataBlob.GetInstance();

        public ActionResult Index()
        {
            
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("CourseList", "Home");

            //return View();
        }

        private bool HasLocalAccount()
        {
            try
            {
                return OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            }
            catch (Exception e)
            {

            }
            return false;
        }

        private string GetMemberId()
        {
            return WebSecurity.GetUserId(User.Identity.Name).ToString();
        }

        private Dictionary<string, int> GetDataAccessControlMap(string jsonString){
            return JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString);
        }

        private Dictionary<string, object> GetMemberInfoLabelMap()
        {
            Dictionary<string, object> mapMember = new Dictionary<string, object>();
            mapMember.Add("MemberID", "Member ID");
            mapMember.Add("NameEN", "Name");
            mapMember.Add("NameCH", "Chinese Name");
            mapMember.Add("Gender", "Gender");
            mapMember.Add("MailAddress", "Mailing Address");
            mapMember.Add("WorkAddress", "Work Address");
            mapMember.Add("EMail", "E-Mail");
            mapMember.Add("DOB", "Date of Birth");
            mapMember.Add("Occupation", "Occupation");
            mapMember.Add("Phone", "Phone");
            mapMember.Add("ProfilePic", "Profile Pic");

            return mapMember;
        }

        private Dictionary<string, object> GetMemberInfoMap(Member m)
        {
            Dictionary<string, object> mapMember = new Dictionary<string, object>();
            mapMember.Add("MemberID", m.MemberID);
            mapMember.Add("NameEN", m.NameEN);
            mapMember.Add("NameCH", m.NameCH);
            mapMember.Add("Gender", m.Gender);
            mapMember.Add("MailAddress", m.MailAddress);
            mapMember.Add("WorkAddress", m.WorkAddress);
            mapMember.Add("EMail", m.EMail);
            mapMember.Add("DOB", m.DOB);
            mapMember.Add("Occupation", m.Occupation);
            mapMember.Add("Phone", m.Phone);
            mapMember.Add("ProfilePic", "");
            mapMember.Add("DataAccessControlMap", GetDataAccessControlMap(m.DataAccessControl));
            mapMember.Add("MemberStatus", m.MemberStatus);

            return mapMember;
        }

        public ActionResult CourseList(
            string userid2, string ne, string nc, string gd, string dob,
            string em, string ma, string ph, string oc, string wa, string dac, 
            string feedback)
        {

            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userid = GetMemberId();


            Member m = ADM.GetMemberById(userid);



            //ViewBag.Message = "Your course list page!";

            if (userid != null && ne != null && nc != null && gd != null && dob != null
                && em != null && ma != null && ph != null && oc != null && wa != null && dac != null)
            {
                ADM.UpdateMember_DataAccessControl(userid, ne, nc, gd, dob, em, ma, ph, oc, wa, dac);
            }

            if (Member.Status.Init.ToString().Equals(m.MemberStatus))
            {
                return RedirectToAction("Profile", "Home");
            }



            Dictionary<string, int> t3 = new Dictionary<string, int>();
            IQueryable<Pending> requestedPending = ADP.GetTargetedMembersA_RequestedPending(userid);
            foreach (var q in requestedPending)
            {
                Member m2 = ADM.GetMemberById(q.BMemberID);
                string[] s = { m2.MemberID, userid };
                Array.Sort(s, StringComparer.InvariantCulture);
                if (!t3.ContainsKey(s[0] + "|" + s[1]))
                {
                    t3[s[0] + "|" + s[1]] = 0;
                }
            }
            requestedPending = ADP.GetTargetedMembersB_RequestedPending(userid);
            foreach (var q in requestedPending)
            {
                Member m2 = ADM.GetMemberById(q.AMemberID);
                string[] s = { m2.MemberID, userid };
                Array.Sort(s, StringComparer.InvariantCulture);

                if (t3.ContainsKey(s[0] + "|" + s[1]))
                {
                    ADA.AddAccepted(s[0], s[1]);
                    ADP.DeletePending(s[0], s[1]);
                }
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


            
            ViewBag.Myself = GetMemberInfoMap(m);
            ViewBag.MemberInfoLabel = GetMemberInfoLabelMap();

            ViewBag.Feedback = feedback;

            return View();
        }

        public ActionResult CourseInfo(string userid2, string courseCode)
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userid = GetMemberId();

            ViewBag.Message = "Your course info page: " + courseCode;

            ViewBag.EnrollmentInfo = "";
            Enrollment en = ADE.GetEnrollmentByMemberIdAndCourseId(userid, courseCode);
            if (en != null)
            {
                ViewBag.EnrollmentInfo = en.EnrollStatus;
            }

            Course c = ADC.GetCourseById(courseCode);
            ViewBag.CourseID = c.CourseID;
            ViewBag.Name = c.Name;
            ViewBag.Description = c.Description;
            ViewBag.DateStart = c.DateStart;
            ViewBag.DateEnd = c.DateEnd;
            ViewBag.NumOfStudent = c.NumOfStudent;
            ViewBag.CourseDateTime = c.CourseDateTime;
            ViewBag.Instructor = c.Instructor;

            return View();
        }

        public ActionResult CourseMediaResource(string courseid)
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userid = GetMemberId();

            ViewBag.Message = "Your course media resource page.";


            //Find course media resources
            List<MediaResource> mediaResourcesVideo = new List<MediaResource>();
            List<MediaResource> mediaResourcesPhoto = new List<MediaResource>();
            List<MediaResource> mediaResourcesOther = new List<MediaResource>();
            IQueryable<CourseMediaRelation> courseMediaRelations;
            MediaResource r;
            if (courseid != null)
            {
                courseMediaRelations = ADCMR.GetCourseMediaRelationByCourseId(courseid);
                foreach (var q in courseMediaRelations)
                {
                    r = ADMR.GetMediaResourceById(q.MediaID);
                    if (MediaResource.TypeMedia.VideoHttp.ToString().Equals(r.MediaType))
                    {
                        mediaResourcesVideo.Add(r);
                    }
                    else if (MediaResource.TypeMedia.PhotoHttp.ToString().Equals(r.MediaType))
                    {
                        mediaResourcesPhoto.Add(r);
                    }
                    else
                    {
                        mediaResourcesOther.Add(r);
                    }                    
                }
            }

            ViewBag.MediaResourcesVideo = mediaResourcesVideo;
            ViewBag.MediaResourcesPhoto = mediaResourcesPhoto;
            ViewBag.MediaResourcesOther = mediaResourcesOther;

            return View();
        }

        public ActionResult CourseClassmateList(string userid2, string courseid, string requestmemberid)
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userid = GetMemberId();


            ViewBag.Message = "Your course classmate list page.";

            //Find classmates
            List<Dictionary<string, object>> members = new List<Dictionary<string, object>>();
            IQueryable<Enrollment> enrollments;
            Member m;
            if (courseid != null)
            {  
                enrollments = ADE.GetEnrollmentsByCourseId(courseid);
                foreach (var q in enrollments)
                {
                    if (!userid.Equals(q.MemberID)){
                        m = ADM.GetMemberById(q.MemberID);
                        members.Add(GetMemberInfoMap(m));               
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

                    if (!t2.ContainsKey(m.MemberID))
                    {
                        t2.Add(m.MemberID, null);
                    }
                }
            }

            ViewBag.Members = members;
            ViewBag.MemberInfoLabel = GetMemberInfoLabelMap();
            ViewBag.FriendsRequestedPendingHashTable = t;
            ViewBag.FriendsAcceptedHashTable = t2;
            ViewBag.CourseID = courseid;

            return View();
        }

        public ActionResult FriendMadeList(string userid2, string acceptmemberid)
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userid = GetMemberId();


            ViewBag.Message = "Your course friend list page.";

            //Find classmates
            List<Dictionary<string, object>> members = new List<Dictionary<string, object>>();
            IQueryable<Pending> friendships;
            IQueryable<Accepted> accepteds;
            Member m;
            if (userid != null)
            {
                friendships = ADP.GetTargetedMembersA_RequestedPending(userid);
                foreach (var q in friendships)
                {
                    m = ADM.GetMemberById(q.BMemberID);
                    members.Add(GetMemberInfoMap(m));
                }

                friendships = ADP.GetTargetedMembersB_RequestedPending(userid);
                foreach (var q in friendships)
                {
                    m = ADM.GetMemberById(q.AMemberID);
                    members.Add(GetMemberInfoMap(m));
                }

                accepteds = ADA.GetAccepteds(userid);
                foreach (var q in accepteds)
                {
                    if (userid.Equals(q.MemberID1))
                    {
                        m = ADM.GetMemberById(q.MemberID2);
                        members.Add(GetMemberInfoMap(m));
                    }
                    else
                    {
                        m = ADM.GetMemberById(q.MemberID1);
                        members.Add(GetMemberInfoMap(m));
                    }
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

                    if (!t.ContainsKey(m.MemberID))
                    {
                        t.Add(m.MemberID, null);
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
            ViewBag.MemberInfoLabel = GetMemberInfoLabelMap();
            ViewBag.FriendsFriendedAcceptedHashTable = t;
            ViewBag.FriendsRequestedPendingHashTable = t2;


            return View();
        }

        public ActionResult Profile(string userid2)
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userid = GetMemberId();

            ViewBag.Message = "Your profile page.";

            Member m = ADM.GetMemberById(userid);
            
            ViewBag.TargetMember = GetMemberInfoMap(m);
            ViewBag.MemberInfoLabel = GetMemberInfoLabelMap();


            return View();
        }

        public ActionResult Image(string id)
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = id;
            if (userId == null)
            {
                userId = GetMemberId();
            }


            string imageId = "pf_" + userId;
            byte[] imageByteArray = ADB.GetBlob(imageId);


            if (imageByteArray == null)
            {
                return RedirectToAction("profile-photo.jpg", "Images");
            }
            else {
                Response.ContentType = "image/jpeg";
                Response.BinaryWrite((byte[])imageByteArray);
                Response.End();
            }


            return View();
        }

        /*
         * 
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            ADM.CreateTable();
            ADC.CreateTable();
            ADE.CreateTable();
            ADP.CreateTable();
            ADA.CreateTable();
            ADMR.CreateTable();
            ADCMR.CreateTable();

            ADB.CreateBlobStorage();
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult UploadProfileImage()
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Index", "Home");
            }

            string userId = GetMemberId();

            try
            {
                var file = Request.Files["file"];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var dirUpload = Server.MapPath("./Uploads/"+userId+"/");

                    Directory.CreateDirectory(dirUpload);

                    var path = Path.Combine(dirUpload, fileName);
                    file.SaveAs(path);
                    
                    ADB.UploadBlob("pf_" + userId, path);


                    DirectoryInfo directory = new DirectoryInfo(dirUpload);
                    foreach (FileInfo file2 in directory.GetFiles()) file2.Delete();
                    directory.Delete(true);
                    
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Profile", "Home");
        }
        */

        public ActionResult UploadProfile()
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = GetMemberId();

            string ne = Request["NameEN"];
            string nc = Request["NameCH"];
            string gd = Request["Gender"];
            string dob = Request["DOB"];
            string em = Request["EMail"];
            string ma = Request["MailAddress"];
            string ph = Request["Phone"];
            string oc = Request["Occupation"];
            string wa = Request["WorkAddress"];
            string dac = Request["dac"];

            string fbk = Request["feedback"];

            if (userId != null && ne != null && nc != null && gd != null && dob != null
                && em != null && ma != null && ph != null && oc != null && wa != null && dac != null)
            {
                ADM.UpdateMember_DataAccessControl(userId, ne, nc, gd, dob, em, ma, ph, oc, wa, dac);
            }

            try
            {
                var file = Request.Files["file"];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var dirUpload = Server.MapPath("./Uploads/" + userId + "/");

                    Directory.CreateDirectory(dirUpload);

                    var path = Path.Combine(dirUpload, fileName);
                    file.SaveAs(path);

                    ADB.UploadBlob("pf_" + userId, path);


                    DirectoryInfo directory = new DirectoryInfo(dirUpload);
                    foreach (FileInfo file2 in directory.GetFiles()) file2.Delete();
                    directory.Delete(true);

                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("CourseList", "Home", new { feedback = fbk });
        }

        public ActionResult CourseSubmit()
        {
            if (!HasLocalAccount())
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = GetMemberId();


            string courseid = Request["courseid"];
            string coursepin = Request["coursepin"];

            string feedback = "";

            Course course = ADC.GetCourseById(courseid);
            if (course.CoursePIN.Equals(coursepin))
            {
                Enrollment enrollment = ADE.GetEnrollmentByMemberIdAndCourseId(userId, courseid);
                if (enrollment == null)
                {
                    ADE.AddEnrollment(courseid, Enrollment.Status.Past.ToString(), userId);
                    feedback = "Course added.";
                }
                else
                {
                    feedback = "Your enrollment record is not found.";
                }
            }

            ViewBag.Feedback = feedback;
            ViewBag.CourseId = courseid;
            return View();
        }
    }
}
