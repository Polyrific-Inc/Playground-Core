// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PG.Model;

namespace PG.DataAccess.EntityTypeConfigs
{
    public class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(profile => profile.AppUser)
                .WithOne(user => user.UserProfile)
                .HasForeignKey<UserProfile>(profile => profile.AppUserId)
                .IsRequired(false);
        }
    }
}