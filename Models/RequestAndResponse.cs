using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Models
{
    public class LoginResponse
    {
        
        public int Status { get; set; }
        public string Text { get; set; }
    }
    public class GetClassByCourse
    {
        [Required]
        public string CourseID { get; set; }
    }
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
    }

    public class NewCourseRequest
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Faculty { get; set; }
        [Required]
        public string UserID { get; set; }
    }

    public class NewClassRequest
    {
        [Required]
        public string FromDate { get; set; }
        [Required]
        public string ToDate { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string CourseID { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string UserID { get; set; }

    }
    public class GetcodeRequest
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        public string ClassID { get; set; }
    }
}
