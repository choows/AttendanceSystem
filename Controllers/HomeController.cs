using AttendanceSystem.Functions;
using AttendanceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            Sessions sessions = new Sessions(HttpContext);
            sessions.RemoveSession("UserID");
            sessions.RemoveSession("UserRole");

            return View();
        }
        public IActionResult ClassDetail(string ClassID)
        {
            AttendanceContext attendanceContext = new AttendanceContext();
            //var sel_class = attendanceContext.classes.Where(x => x.ID.ToString() == ClassID).FirstOrDefault();
            var sel_class = (from c in attendanceContext.classes
                             join cour in attendanceContext.courses on c.Course equals cour
                             where c.ID.ToString() == ClassID
                             select new Class()
                             {
                                 ID = c.ID,
                                 AllowCheckin = c.AllowCheckin,
                                 Course = cour,
                                 CreateBy = c.CreateBy,
                                 Description = c.Description,
                                 FromTime = c.FromTime,
                                 Location = c.Location,
                                 Token = c.Token,
                                 ToTime = c.ToTime,
                                 Type = c.Type
                             }).FirstOrDefault();
            Sessions sessions = new Sessions(HttpContext);
            string UserID = sessions.GetStringSession("UserID");
            ViewBag.SelClass = sel_class;
            ViewBag.UserID = UserID;
            return View();
        }
        public IActionResult ClassManagement()
        {
            Sessions sessions = new Sessions(HttpContext);
            string UserID = sessions.GetStringSession("UserID");
            AttendanceContext attendanceContext = new AttendanceContext();
            var user = attendanceContext.Users.Where(x => x.ID == UserID).FirstOrDefault();
            var courses = attendanceContext.courses.ToList();
            var classes = attendanceContext.classes.Where(x=> x.CreateBy == user).ToList();
            ViewBag.Classes = classes;
            ViewBag.UserId = UserID;
            ViewBag.Courses = courses;
            return View();
        }
        public IActionResult CoursesManagement()
        {
            Sessions sessions = new Sessions(HttpContext);
            string UserID = sessions.GetStringSession("UserID");
            AttendanceContext attendanceContext = new AttendanceContext();
            var courses = attendanceContext.courses.ToList();
            ViewBag.Courses = courses;
            ViewBag.UserId = UserID;
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Index()
        {
           return RedirectToAction("ClassManagement");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
