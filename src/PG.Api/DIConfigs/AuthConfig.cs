using Microsoft.Extensions.DependencyInjection;
using PG.Api.Auth;

namespace PG.Api.DIConfigs
{
    public static class AuthConfig
    {
        public static void RegisterAuthService(this IServiceCollection services)
        {
            services.AddSingleton<IJwtAuthFactory, JwtAuthFactory>();
        }
    }
}
