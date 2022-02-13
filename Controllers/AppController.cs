using AttendanceSystem.Functions;
using AttendanceSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Controllers
{
    [AllowAnonymous]
    public class AppController : Controller
    {
        private readonly ILogger<AppController> _logger;

        public AppController(ILogger<AppController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> WebLogin ([FromBody] LoginRequest req)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                Encryption encryption = new Encryption();
                string EncryptedPas = encryption.CreateMD5Hash(req.Password);
                var user = attendanceContext.Users.Where(x => x.UserName.ToLower() == req.UserName.ToLower() && x.Password == EncryptedPas && x.Active).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("Incorrect UserName or Password");
                }
                else
                {
                    if(user.Role != Functions.Constants.Role_Lec)
                    {
                        throw new Exception("Only Lecturer are able to login to the website.");
                    }
                    Sessions sessions = new Sessions(HttpContext);
                   sessions.SetStringSession("UserID", user.ID);
                    sessions.SetStringSession("UserRole", user.Role);
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, req.UserName));
                    var identity = new ClaimsIdentity(claims, "LoginClaim");
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(50)
                        });
                    return Json(new { status = 200, text = "Login Success" });
                }
            }
            catch (Exception exp)
            {

                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "Login Failed" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string UserName, string Password)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                Encryption encryption = new Encryption();
                string EncryptedPas = encryption.CreateMD5Hash(Password);
                var user = attendanceContext.Users.Where(x => x.UserName.ToLower() == UserName.ToLower() && x.Password == EncryptedPas && x.Active).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("Incorrect UserName or Password");
                }
                else
                {
                    return Json(new { status = 200, text = "Login Success" });
                }
            }
            catch (Exception exp)
            {

                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "Login Failed" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterRequest req)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                Encryption encryption = new Encryption();
                string EncPass = encryption.CreateMD5Hash(req.Password);
                var usr = attendanceContext.Users.Where(x => x.ID.ToLower() == req.ID.ToLower()).FirstOrDefault();
                if (usr != null)
                {
                    throw new Exception("User Existed");
                }
                using (var trans = attendanceContext.Database.BeginTransaction())
                {
                    await attendanceContext.Users.AddAsync(new Users()
                    {
                        ID = req.ID,
                        Active = true,
                        IpAddress = string.Empty,
                        LastLoginTime = null,
                        Name = req.Name,
                        Password = EncPass,
                        Role = req.Role,
                        UserName = req.UserName
                    });
                    await attendanceContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                return Json(new { status = 200, text = "Done Register" });
            }
            catch (Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "Register Failed" });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewCourse([FromBody] NewCourseRequest req)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                using (var trans = attendanceContext.Database.BeginTransaction())
                {
                    var ext_course = attendanceContext.courses.Where(x => x.ID.ToLower() == req.ID.ToLower()).FirstOrDefault();
                    if (ext_course != null)
                    {
                        throw new Exception("Course Existed.");
                    }
                    var course = new Course()
                    {
                        ID = req.ID,
                        Active = true,
                        Faculty = req.Faculty,
                        Name = req.Name
                    };
                    attendanceContext.courses.Attach(course);
                    await attendanceContext.courses.AddAsync(course);
                    await attendanceContext.Registered_course.AddAsync(new Registered_Course()
                    {
                        Course = course,
                        ID = Guid.NewGuid(),
                        User = attendanceContext.Users.Where(x => x.ID == req.UserID).FirstOrDefault()
                    }) ;

                    await attendanceContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                return Json(new { status = 200, text = "done Added Course" });
            }
            catch (Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "add course failed" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCourse(string UserID, string CourseID)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                var course = attendanceContext.courses.Where(x => x.ID == CourseID).FirstOrDefault();
                var user = attendanceContext.Users.Where(x => x.ID == UserID).FirstOrDefault();
                if (course == null)
                    throw new Exception("Course Not found");
                if (user == null)
                    throw new Exception("User Not found");
                var users = (from r_c in attendanceContext.Registered_course
                             join c in attendanceContext.courses on r_c.Course equals c
                             join u in attendanceContext.Users on r_c.User equals u
                             where c == course
                             select u).ToList();
                if (user.Role == Constants.Role_Lec && users.Where(x => x.Role == Constants.Role_Lec).FirstOrDefault() != null)
                {
                    throw new Exception("Lecturer Already Registered For the course");
                }
                using (var trans = attendanceContext.Database.BeginTransaction())
                {
                    await attendanceContext.Registered_course.AddAsync(new Registered_Course()
                    {
                        Course = course,
                        ID = Guid.NewGuid(),
                        User = user
                    });
                    await attendanceContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                return Json(new { status = 200, text = "Register Course Success" });
            }
            catch (Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "Register Course Failed" });
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UnRegisterCourse(string UserID, string CourseID)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                var course = attendanceContext.courses.Where(x => x.ID == CourseID).FirstOrDefault();
                var user = attendanceContext.Users.Where(x => x.ID == UserID).FirstOrDefault();
                if (course == null)
                    throw new Exception("Course Not found");
                if (user == null)
                    throw new Exception("User Not found");
                var users = (from r_c in attendanceContext.Registered_course
                             join c in attendanceContext.courses on r_c.Course equals c
                             join u in attendanceContext.Users on r_c.User equals u
                             where c == course
                             select u).ToList();
                var ext = attendanceContext.Registered_course.Where(x => x.User == user && x.Course == course).FirstOrDefault();
                if (ext == null)
                {
                    throw new Exception("User Not Registered to the course");
                }
                
                using (var trans = attendanceContext.Database.BeginTransaction())
                {
                    attendanceContext.Registered_course.Remove(ext);
                    await attendanceContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                return Json(new { status = 200, text = "Register Course Success Removed" });
            }
            catch (Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "Register Course Failed Removed" });
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewClass([FromBody] NewClassRequest req)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                var course = attendanceContext.courses.Where(x => x.ID == req.CourseID).FirstOrDefault();
                var user = attendanceContext.Users.Where(x => x.ID == req.UserID && x.Role == Constants.Role_Lec).FirstOrDefault();
                if (course == null)
                    throw new Exception("Course Not found");
                if (user == null)
                    throw new Exception("User Not Found");
                DateTime fromDate = DateTime.Parse(req.FromDate);
                DateTime toDate = DateTime.Parse(req.ToDate);
                if(toDate <= fromDate)
                {
                    throw new Exception("Invalid Date ");
                }
                var ext_class = attendanceContext.classes.Where(x => x.Course == course && x.FromTime == fromDate && x.ToTime == toDate && x.Location == req.Location).FirstOrDefault();
                if(ext_class != null)
                {
                    throw new Exception("Class Registered");
                }
                using(var trans = attendanceContext.Database.BeginTransaction())
                {
                    var classes = new Class()
                    {
                        ID = Guid.NewGuid(),
                        AllowCheckin = true,
                        Course = course,
                        CreateBy = user,
                        Description = req.Description,
                        FromTime = fromDate,
                        Location = req.Location,
                        Token = string.Empty,
                        ToTime = toDate,
                        Type = req.Type
                    };
                    await attendanceContext.classes.AddAsync(classes);

                    await attendanceContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                return Json(new { status = 200, text = "Success Create New Class" });
            }
            catch(Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "Add New Class Failed" });
            }
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult GetCode([FromBody] GetcodeRequest req)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                string NewToken = TokenGenerator.RandomString(10);
                var user = attendanceContext.Users.Where(x => x.ID == req.UserID && x.Role == Constants.Role_Lec).FirstOrDefault();
                if (user == null)
                    throw new Exception("User Not found");
                var sel_class = (from c in attendanceContext.classes
                                 join cour in attendanceContext.courses on c.Course equals cour
                                 where c.ID.ToString() == req.ClassID
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
                                     ToTime = c.ToTime ,
                                     Type = c.Type
                                 }).FirstOrDefault();

                // var sel_class = attendanceContext.classes.Include(x => x.Course).Where(x => x.ID.ToString() == req.ClassID).FirstOrDefault();
                if (sel_class == null)
                    throw new Exception("Class not found");
                var reg = attendanceContext.Registered_course.Where(x => x.Course == sel_class.Course && x.User == user).FirstOrDefault();
                if(reg == null)
                {
                    throw new Exception("Course Not Registered");
                }
                using(var trans = attendanceContext.Database.BeginTransaction())
                {
                    var c = attendanceContext.classes.Where(x => x == sel_class).FirstOrDefault();
                    c.Token = NewToken;
                    attendanceContext.SaveChanges();
                    trans.Commit();
                    return Json(new { status = 200, token = NewToken, text = "Generated" });
                }

            }catch(Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, token = string.Empty, text = "failed" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Attend(string UserId , string ClassID , string Token)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                var sel_class = attendanceContext.classes.Where(x => x.ID.ToString() == ClassID && x.Token == Token).FirstOrDefault();
                if (sel_class == null)
                    throw new Exception("Attemp failed");
                var user = attendanceContext.Users.Where(x => x.ID == UserId).FirstOrDefault();
                if (user == null)
                    throw new Exception("User Not Found");
                using (var trans = attendanceContext.Database.BeginTransaction())
                {
                    var ext = attendanceContext.checkIns.Where(x => x.User == user && x.Class == sel_class).FirstOrDefault();
                    if (ext != null)
                        throw new Exception("Already Exist");
                    await attendanceContext.checkIns.AddAsync(new CheckIn()
                    {
                        CheckinDateTime = DateTime.Now,
                        IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        Class = sel_class,
                        ID = Guid.NewGuid(),
                        User = user
                    });
                    await attendanceContext.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Json(new { status = 200, text = "done check in" });
                }

            }catch(Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, text = "Attend Failed" });
            }
        }


        //Get Request 
        [HttpGet]
        [Authorize]
        public IActionResult GetStudentByCourse(string CourseID)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                var course = attendanceContext.courses.Where(x => x.ID == CourseID).FirstOrDefault();
                if (course == null)
                    throw new Exception("Course Not Found");
                var st = (from c in attendanceContext.courses
                          join r_c in attendanceContext.Registered_course on c equals r_c.Course
                          join s in attendanceContext.Users on r_c.User equals s
                          where c.ID == CourseID && s.Role == Constants.Role_Student
                          select s).ToList();
                return Json(new { status = 200, lists = st, text = "done" });
            }catch(Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, lists = new List<Users>(), text = "failed" });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetStudentByClass(string ClassID , string UserID)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                var sel_class = attendanceContext.classes.Where(x => x.ID.ToString() == ClassID).FirstOrDefault();
                if (sel_class == null)
                    throw new Exception("Class Not Found");

                var user = attendanceContext.Users.Where(x => x.ID == UserID).FirstOrDefault();
                if(user.Role != Constants.Role_Lec)
                {
                    throw new Exception("UnAuthorized User");
                }
                var Registered = (from rg in attendanceContext.Registered_course
                                  join u in attendanceContext.Users on rg.User equals u
                                  where rg.Course == sel_class.Course && u != user
                                  select u).ToList();
                var Attended = attendanceContext.checkIns.Include(x => x.User).Where(x => x.Class == sel_class).Select(x => x.User).ToList();
                Registered.RemoveAll(x => Attended.Contains(x));

                return Json(new { status = 200 , attended_list = Attended , unattended_list = Registered , text = "done"});
            }catch(Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400, attended_list = new List<Users>() , unattended_list = new List<Users>(), text = "failed" });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetClassByCourse([FromBody] GetClassByCourse req)
        {
            try
            {
                AttendanceContext attendanceContext = new AttendanceContext();
                var course = attendanceContext.courses.Where(x => x.ID == req.CourseID).FirstOrDefault();
                if(course == null)
                {
                    throw new Exception("Course Not Found");
                }

                var classes = attendanceContext.classes.Where(x => x.Course == course).ToList();
                return Json(new { status = 200, classes = classes });
            }catch(Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return Json(new { status = 400 , classes = new List<Class> ()});
            }
        }
    }
}
