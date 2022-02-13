using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Models
{
    public class AttendanceContext : DbContext
    {
        public DbSet<CheckIn> checkIns { get; set; }
        public DbSet<Class> classes { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Registered_Course> Registered_course { get; set; }
        public DbSet<Users> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=AttendanceSystem;Integrated Security=SSPI;");
        }
    }
}
