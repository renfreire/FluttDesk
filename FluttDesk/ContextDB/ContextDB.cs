using FluttDesk.Models;
using Microsoft.EntityFrameworkCore;

namespace FluttDesk.ContextDB
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options) => Database.EnsureCreated();

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<DailyDedication> DailyDedications { get; set; }
        public DbSet<Status_activities> Status_activities { get; set; }

        public DbSet<Systems> Systems { get; set; }
        public DbSet<ActivitiesProject> ActivitiesProject { get; set; }
        public DbSet<Projects> Projects { get; set; }

        public DbSet<Users> Users { get; set; }


    }
}
