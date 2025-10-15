
using Assert.Application.DTOs.Requests;
using Assert.Application.Interfaces;
using Assert.Application.Interfaces.Notifications;
using Assert.Application.Mappings;
using Assert.Application.Services;
using Assert.Application.Services.Notifications;
using Assert.Application.Services.Security;
using Assert.Application.Validators;
using Assert.Domain.Implementation;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
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
            //services.AddSingleton<IUrlBaseService, UrlBaseService>();

            // AutoMapper
            services.AddAutoMapper(typeof(AutomapperProfile));

            // Mapeador personalizado
            services.AddScoped<CustomProfileMapper>();

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
            services.AddValidatorsFromAssemblyContaining<LocalUserRequestValidator>();
            services.AddScoped<IValidator<UpdatePersonalInformationRequest>,
                PersonalInformationBaseValidator<UpdatePersonalInformationRequest>>();


            //Application Services
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IAppListingRentService, AppListingRentService>();
            services.AddScoped<IAppSearchService, AppSearchService>();
            services.AddScoped<IAppListingFavoriteService, AppListingFavoriteService>();
            services.AddScoped<IAppParametricService, AppParametricService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppMessagingService, AppMessagingService>();
            services.AddScoped<IAppListingCalendarService, AppListingCalendarService>();
            services.AddScoped<IAppBookService, AppBookService>();
            services.AddScoped<IAppMethodOfPaymentService, AppMethodOfPaymentService>();
            services.AddScoped<IAppReviewService, AppReviewService>();
            services.AddScoped<IAppNotificationService, AppNotificationService>();
            services.AddScoped<IAppCompliantService, AppCompliantService>();
            services.AddScoped<IAppDashboardService, AppDashboardService>();
            services.AddScoped<IEmailNotificationService, EmailNotificationsService>();


            //Domain Services
            services.AddScoped<IListingRentService, ListingRentService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IListingFavoriteService, ListingFavoriteService>();
            services.AddScoped<IParametricService, ParametricService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILocationSugestionService, LocationSugestionService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IMethodOfPaymentService, MethodOfPaymentService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IComplaintService, ComplaintService>();

            return services;
        }
    }
}
