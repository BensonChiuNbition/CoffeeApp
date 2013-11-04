using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeAppWebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return RedirectToAction("Login", "Account");
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CourseList()
        {
            ViewBag.Message = "Your course list page.";

            return View();
        }

        public ActionResult CourseInfo(string courseCode)
        {
            ViewBag.Message = "Your course info page: " + courseCode;
            ViewBag.CourseDescription = "Welcome to this course!";
            return View();
        }

        public ActionResult CourseMediaResource()
        {
            ViewBag.Message = "Your course media resource page.";

            return View();
        }

        public ActionResult CourseClassmateList()
        {
            ViewBag.Message = "Your course classmate list page.";

            return View();
        }

        public ActionResult FriendMadeList()
        {
            ViewBag.Message = "Your course friend list page.";

            return View();
        }

        public ActionResult Profile()
        {
            ViewBag.Message = "Your profile page.";
            return View();
        }
    }
}
