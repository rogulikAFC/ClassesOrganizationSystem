using ClassesOrganizationSystem.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassesOrganizationSystem.Infrastructure.Persistence
{
    public static class PersistenceDI
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddDbContext<ClassesOrganizationSystemDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ClassesOrganizationSystemDb")));

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
