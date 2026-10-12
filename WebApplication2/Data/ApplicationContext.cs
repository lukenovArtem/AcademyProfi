using Microsoft.EntityFrameworkCore;
using AcademyProfi.Models;

namespace AcademyProfi.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<StatusDict> StatusDicts { get; set; }
    }
}