using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Onboarding.Models
{
    public class OnboardingContext : DbContext
    {
        public OnboardingContext (DbContextOptions<OnboardingContext> options)
            : base(options)
        {
        }

        public DbSet<Onboarding.Models.UserAccount> UserAccount { get; set; }
        public DbSet<Onboarding.Models.Workspace> Workspace { get; set; }
        public DbSet<Onboarding.Models.UserState> UserState { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserWorkspace>()
                .HasKey(uw => new { uw.UserId, uw.WorkspaceId });

            modelBuilder.Entity<UserWorkspace>()
                .HasOne(u => u.UserAccount)
                .WithMany(u => u.UserWorkspaces)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserWorkspace>()
                .HasOne(w => w.Workspace)
                .WithMany(c => c.UserWorkspaces)
                .HasForeignKey(w => w.WorkspaceId);
        }
    }   
}
