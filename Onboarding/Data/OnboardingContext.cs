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
    }
}
