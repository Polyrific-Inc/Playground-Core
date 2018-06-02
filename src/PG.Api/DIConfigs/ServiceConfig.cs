// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using PG.BLL;

namespace PG.Api.DIConfigs
{
    public static class ServiceConfig
    {
        public static void RegisterAppServices(this IServiceCollection services)
        {
            services.AddTransient<IFacilityService, FacilityService>();
            services.AddTransient<ISiteService, SiteService>();
        }
    }
}