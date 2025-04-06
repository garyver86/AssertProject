
using Assert.Application.Interfaces;
using Assert.Application.Services;
using Assert.Application.Services.Security;
using Assert.Domain.Implementation;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Infrastructure.InternalServices;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using Assert.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Application
{
    public static class ApplicationInjectionDependences
    {
        public static IServiceCollection AddApplicationInjections(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(AutomapperProfile));

            //Application Services
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IAppListingRentService, AppListingRentService>();
            services.AddScoped<IAppSearchService, AppSearchService>();
            services.AddScoped<IAppListingFavoriteService, AppListingFavoriteService>();
            services.AddScoped<IAppParametricService, AppParametricService>();
            services.AddScoped<IAppUserService, AppUserService>();

            //Domain Services
            services.AddScoped<IListingRentService, ListingRentService>();
            services.AddScoped<IListingFavoriteService, ListingFavoriteService>();
            services.AddScoped<IParametricService, ParametricService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
