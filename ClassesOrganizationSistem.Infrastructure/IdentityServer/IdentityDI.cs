using ClassesOrganizationSistem.Domain.Entities.UserEntites;
using ClassesOrganizationSistem.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ClassesOrganizationSistem.Infrastructure.IdentityServer
{
    public static class IdentityDI
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
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
