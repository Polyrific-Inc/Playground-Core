// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using PG.Repository;

namespace PG.Api.DIConfigs
{
    public static class RepositoryConfig
    {
        public static void RegisterAppRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        }
    }
}