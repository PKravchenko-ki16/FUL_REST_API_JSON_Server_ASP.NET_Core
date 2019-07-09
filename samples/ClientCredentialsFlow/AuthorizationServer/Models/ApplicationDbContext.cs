using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {}
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Bound> Bound { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {}
    }
}
