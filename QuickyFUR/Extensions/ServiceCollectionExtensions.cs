using Microsoft.EntityFrameworkCore;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Services;
using QuickyFUR.Infrastructure.Data;
using QuickyFUR.Infrastructure.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDesignerService, DesignerService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddControllersWithViews().AddSessionStateTempDataProvider();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(@"Server=.;Database=QuickyFUR;Trusted_Connection=True;"));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
