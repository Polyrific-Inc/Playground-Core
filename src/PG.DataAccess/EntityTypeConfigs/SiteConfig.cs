// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PG.Model;

namespace PG.DataAccess.EntityTypeConfigs
{
    public class SiteConfig : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            
        }
    }
}