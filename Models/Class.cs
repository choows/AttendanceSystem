using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Models
{
    public class Class
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; }
        public Course Course { get; set; }
        [Column (TypeName = "varchar(400)")]
        public string Description { get; set; }
        public Users CreateBy { get; set; }
        public bool AllowCheckin { get; set; }
        [Column (TypeName = "varchar(100)")]
        public string Token { get; set; }
        [Column (TypeName = "varchar(100)")]
        public string Location { get; set; }
    }
}
