using Microsoft.EntityFrameworkCore;
using erp.Models;

namespace erp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamMember>().ToTable("team_members");
            modelBuilder.Entity<Employee>().ToTable("employees");
        }
    }
}
