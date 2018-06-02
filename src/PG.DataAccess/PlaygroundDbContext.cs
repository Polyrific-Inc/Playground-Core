// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using PG.DataAccess.EntityTypeConfigs;
using PG.Model;

namespace PG.DataAccess
{
    public class PlaygroundDbContext : DbContext
    {
        public PlaygroundDbContext(DbContextOptions<PlaygroundDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Site> Sites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FacilityConfig());
            modelBuilder.ApplyConfiguration(new SiteConfig());
        }
    }
}