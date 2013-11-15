using CoffeeAppWebRole.DAO;
using CoffeeAppWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.ActionData
{
    public class ActionDataEnrollment
    {
        private static ActionDataEnrollment Instance;

        private static TableStorageContextEnrollments Context = new TableStorageContextEnrollments();

        protected ActionDataEnrollment() {
        }

        public static ActionDataEnrollment GetInstance()
        {
            if (Instance == null) {
                Instance = new ActionDataEnrollment();
            }
            return Instance;
        }

        public void AddEnrollment(string courseId, string enrollStatus, string memberId)
        {
            var enrollment = new Enrollment(){
                CourseID = courseId,
                EnrollStatus = enrollStatus,
                MemberID = memberId
            };
            Context.AddEnrollment(enrollment);
        }

        public Enrollment GetEnrollmentByMemberIdAndCourseId(string userid, string courseId)
        {
            return Context.GetEnrollmentByMemberIdAndCourseId(userid, courseId);
        }
        public IQueryable<Enrollment> GetEnrollmentsByMemberId(string userid)
        {
            return Context.GetEnrollmentsByMemberId(userid);
        }

        public IQueryable<Enrollment> GetEnrollmentsByCourseId(string courseid)
        {
            return Context.GetEnrollmentsByCourseId(courseid);
        }

        public void CreateTable()
        {
            if (!TableStorageContextAccepteds.IsTableExisted())
            {
                TableStorageContextEnrollments.CreateTableIfNotExist();

                var enrollment = new Enrollment()
                {
                    //EnrollmentID = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    MemberID = "1",
                    CourseID = "COFA03",
                    EnrollStatus = Enrollment.Status.None.ToString()
                };
                var enrollment2 = new Enrollment()
                {
                    //EnrollmentID = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    MemberID = "1",
                    CourseID = "COFB09",
                    EnrollStatus = Enrollment.Status.Past.ToString()
                };
                var enrollment3 = new Enrollment()
                {
                    //EnrollmentID = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    MemberID = "2",
                    CourseID = "COFB09",
                    EnrollStatus = Enrollment.Status.Past.ToString()
                };
                Context.AddEnrollment(enrollment);
                Context.AddEnrollment(enrollment2);
                Context.AddEnrollment(enrollment3);
            }
        }
    }
}