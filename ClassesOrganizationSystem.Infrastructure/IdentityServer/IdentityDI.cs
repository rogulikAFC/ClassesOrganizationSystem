using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using ClassesOrganizationSystem.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ClassesOrganizationSystem.Infrastructure.IdentityServer
{
    public static class IdentityDI
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:7290";
                    options.TokenValidationParameters.ValidateAudience = false;
                });

            services.AddAuthorization();

            services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ClassesOrganizationSystemDbContext>();

            services.AddIdentityServer()
                .AddInMemoryClients(Configuration.Clients)
                .AddInMemoryIdentityResources(Configuration.Resources)
                .AddInMemoryApiScopes(Configuration.Scopes)
                .AddAspNetIdentity<User>()
                .AddDeveloperSigningCredential();

            return services;
        }
    }
}
