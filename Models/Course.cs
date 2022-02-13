using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Models
{
    public class Course
    {
        [Key]
        [Column(TypeName = "varchar(100)")]
        public string ID { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Faculty { get; set; }
        public bool Active { get; set; }
    }
}
