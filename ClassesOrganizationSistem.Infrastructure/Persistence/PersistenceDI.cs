using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassesOrganizationSistem.Infrastructure.Persistence
{
    public static class PersistenceDI
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddDbContext<ClassesOrganizationSystemDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ClassesOrganizationSystemDb")));

            return services;
        }
    }
}
