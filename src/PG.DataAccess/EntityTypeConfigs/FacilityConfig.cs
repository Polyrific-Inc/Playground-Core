// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PG.Model;

namespace PG.DataAccess.EntityTypeConfigs
{
    public class FacilityConfig : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.HasOne(facility => facility.Site)
                .WithMany(site => site.Facilities)
                .HasForeignKey(facility => facility.SiteId);
        }
    }
}