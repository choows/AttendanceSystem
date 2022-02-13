using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Models
{
    public class Users
    {
        [Key]
        [Column(TypeName = "varchar(100)")]
        public string ID { get; set; }
        [Column(TypeName="varchar(200)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }

        public DateTime? LastLoginTime { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string IpAddress { get; set; }

        [Column (TypeName = "varchar(500)")]
        public string Role { get; set; }

        public bool Active { get; set; }
    }
}
