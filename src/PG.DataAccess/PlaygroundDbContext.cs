// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PG.DataAccess.EntityTypeConfigs;
using PG.Model;
using PG.Model.Identity;

namespace PG.DataAccess
{
    public class PlaygroundDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public PlaygroundDbContext(DbContextOptions<PlaygroundDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FacilityConfig());
            modelBuilder.ApplyConfiguration(new SiteConfig());
            modelBuilder.ApplyConfiguration(new UserProfileConfig());
        }
    }
}