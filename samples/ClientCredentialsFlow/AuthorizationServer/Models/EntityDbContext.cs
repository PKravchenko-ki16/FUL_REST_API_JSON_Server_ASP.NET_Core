using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.Models
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Bound> Bound { get; set; }

    }
}
