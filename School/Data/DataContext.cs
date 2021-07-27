using Microsoft.EntityFrameworkCore;
using School.Models;

namespace School.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<Student> Student { get; set; }

        public DbSet<Classes> Classes { get; set; }

        public DbSet<StudentClassGrade> StudentClassGrade { get; set; }
    }
}