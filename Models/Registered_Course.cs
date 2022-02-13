using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Models
{
    public class Registered_Course
    {
        [Key]
        public Guid ID { get; set; }
        public Course Course { get; set; }
        public Users User { get; set; }
    }
}
