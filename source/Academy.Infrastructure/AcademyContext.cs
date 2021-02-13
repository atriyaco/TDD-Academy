using Academy.Domain;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure
{
    public class AcademyContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        public AcademyContext(DbContextOptions<AcademyContext> options) : base(options)
        {
        }
    }
}