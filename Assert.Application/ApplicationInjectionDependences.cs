
using Assert.Application.Interfaces;
using Assert.Application.Mappings;
using Assert.Application.Services;
using Assert.Application.Services.Security;
using Assert.Application.Validators;
using Assert.Domain.Implementation;
using Assert.Domain.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
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

            //Validator
            //services.AddFluentValidationAutoValidation();
            //services.AddFluentValidationClientsideAdapters();
            //services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddFluentValidation(options =>
            {
                options.DisableDataAnnotationsValidation = true;
                options.ImplicitlyValidateChildProperties = false;
            });
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

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
