using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Models
{
    public class CheckIn
    {
        [Key]
        public Guid ID { get; set; }
        public Users User { get; set; }
        public Class Class { get; set; }
        public DateTime CheckinDateTime { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string IpAddress { get; set; }
    }
}
