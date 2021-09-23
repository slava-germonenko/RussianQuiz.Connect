using Microsoft.EntityFrameworkCore;

using RussianQuiz.Connect.Core.Models;
using RussianQuiz.Connect.Core.Settings;


namespace RussianQuiz.Connect.Core
{
    public class AuthContext : DbContext
    {
        public DbSet<ServicePlan> ServicePlans { get; set; }

        public DbSet<User> Users { get; set; }


        public AuthContext(IAuthContextSettings settings) : base(GenerateContextSettings(settings)) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<ServicePlan>()
                .ToTable("ServicePlans")
                .HasOne(sp => sp.User)
                .WithMany(u => u.ServicePlans);
        }

        private static DbContextOptions<AuthContext> GenerateContextSettings(IAuthContextSettings settings)
            => new DbContextOptionsBuilder<AuthContext>()
                .UseSqlServer(settings.ConnectionString)
                .Options;
    }
}