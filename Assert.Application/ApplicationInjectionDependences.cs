
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
        public static IServiceCollection ApiInjectionDependences(
            this IServiceCollection services, IConfiguration configuration)
        {



            var connectionString = configuration.GetConnectionString("AssertDB");

            services.AddDbContext<InfraAssertDbContext>(options =>
            options.UseSqlServer(connectionString,
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(20),
                    errorNumbersToAdd: null
                )),
            ServiceLifetime.Scoped);

            services.AddAutoMapper(typeof(AutomapperProfile));

            //Application Services
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IAppListingRentService, AppListingRentService>();
            services.AddScoped<IAppSearchService, AppSearchService>();

            //Infra Services
            services.AddScoped<IJWTSecurity, JWTSecurityService>();
            services.AddScoped<IAuthentication, AuthenticationService>();

            //Domain Services
            services.AddScoped<IListingRentService, ListingRentService>();
            services.AddScoped<IErrorHandler, ErrorHandler>();
            services.AddScoped<ISearchService, SearchService>();

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
            services.AddScoped<IListingrentChangeRepository, ListingRentChangeRepository>();
            services.AddScoped<IListingStatusRepository, ListingStatusRepository>();
            services.AddScoped<IListingRentRepository, ListingRentRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IStepsTypeRepository, StepsTypeRepository>();
            services.AddScoped<IViewTypeRepository, ViewTypeRepository>();
            services.AddScoped<IListingStepViewRepository, ListingStepViewRepository>();
            services.AddScoped<IListingStepRepository, ListingStepRepository>();
            services.AddScoped<IQuickTipRepository, QuickTipRepository>();
            services.AddScoped<IQuickTypeViewRepository, QuickTypeViewRepository>();
            services.AddScoped<IListingLogRepository, ListingLogRepository>();
            services.AddScoped<ITErrorParamRepository, ErrorParamRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IListingSpaceRepository, ListingSpaceRepository>();
            services.AddScoped<IListingAmenitiesRepository, ListingAmenitiesRepository>();
            services.AddScoped<IListingDiscountForRateRepository, ListingDiscountForRateRepository>();
            services.AddScoped<IListingRentRulesRepository, ListingRentRulesRepository>();
            services.AddScoped<IListingPhotoRepository, ListingPhotoRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IListingRent_StepViewService, ListingRent_StepViewService>();
            services.AddScoped<IPropertySubTypeRepository, PropertySubTypeRepository>();
            services.AddScoped<IAccommodationTypeRepository, AccommodationTypeRepository>();
            services.AddScoped<IPropertyAddressRepository, PropertyAddressRepository>();


            return services;
        }
    }
}
