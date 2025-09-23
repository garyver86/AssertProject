using Assert.Domain.Enums;
using Assert.Domain.Implementation;
using Assert.Domain.Interfaces.Infraestructure.External;
using Assert.Domain.Interfaces.Notifications;
using Assert.Domain.Notifications.Settings;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.External.AuthProviderValidator;
using Assert.Infrastructure.External.Notifications;
using Assert.Infrastructure.ExternalServices;
using Assert.Infrastructure.InternalServices;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using Assert.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Assert.Infrastructure;

public static class InfrastructureInjectionDependences
{
    public static IServiceCollection AddInfrastructureInjections(
            this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AssertDB");

        services.AddDbContextPool<InfraAssertDbContext>(options =>
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(20),
                    errorNumbersToAdd: null);
            }));

        //smtp
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<SmtpClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<EmailSettings>>().Value;

            return new SmtpClient(settings.SmtpServer, settings.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.From, settings.Password),
                EnableSsl = true
            };
        });
        services.AddScoped<IEmailNotification, GmailNotification>();

        services.AddScoped<IJWTSecurity, JWTSecurityService>();
        services.AddScoped<IAuthentication, AuthenticationService>();
        services.AddScoped<IErrorHandler, ErrorHandler>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IFuzzyMatcher, FuzzyMatcher>();

        //services.AddSingleton<ISocketIoService, SocketIoService>(); 
        services.AddSingleton<IConnectionManager, SignalRConnectionManager>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationDispatcher, SignalRNotificationDispatcher>();

        services.AddScoped<GoogleAuthValidator>();
        services.AddScoped<AppleAuthValidator>();
        services.AddScoped<MetaAuthValidator>();
        services.AddScoped<LocalAuthValidator>();

        #region auth validator social media
        services.AddScoped<Func<Platform, IAuthProviderValidator>>(serviceProvider => key =>
        {
            return key switch
            {
                Platform.Google => serviceProvider.GetRequiredService<GoogleAuthValidator>(),
                Platform.Apple => serviceProvider.GetRequiredService<AppleAuthValidator>(),
                Platform.Meta => serviceProvider.GetRequiredService<MetaAuthValidator>(),
                Platform.Local => serviceProvider.GetRequiredService<LocalAuthValidator>(),
                _ => throw new InfrastructureException("Proveedor Invalido")
            };
        });
        #endregion

        #region Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRolRepository, UserRolRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
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
        services.AddScoped<ISystemConfigurationRepository, SystemConfigurationRepository>();
        services.AddScoped<IListingFavoriteRepository, ListingFavoriteRepository>();
        services.AddScoped<IListingViewHistoryRepository, ListingViewHistoryRepository>();
        services.AddScoped<IAmenitiesRepository, AmenitiesRepository>();
        services.AddScoped<IFeaturesAspectsRepository, FeaturedAspectsRepository>();
        services.AddScoped<IDiscountTypeRepository, DiscountTypeRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUserTypeRepository, UserTypeRepository>();
        services.AddScoped<IListingFeaturedAspectRepository, ListingFeaturedAspectRepository>();
        services.AddScoped<IListingPricingRepository, ListingPricingRepository>();
        services.AddScoped<IListingDiscountRepository, ListingDiscountRepository>();
        services.AddScoped<IListingRentReviewRepository, ListingRentReviewRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ISpaceTypeRepository, SpaceTypeRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IListingCalendarRepository, ListingCalendarRepository>();
        services.AddScoped<IEmergencyContactRepository, EmergencyContactRepository>();
        services.AddScoped<IPayPriceCalculationRepository, PayPriceCalculationRepository>();
        services.AddScoped<IMethodOfPaymentRepository, MethodOfPaymentRepository>();
        services.AddScoped<ISecurityItemsRepository, SecurityItemsRepository>();
        services.AddScoped<IApprovalPolityTypeRepository, ApprovalPolityTypeRepository>();
        services.AddScoped<IRulesTypeRepository, RuleTypesRepository>();
        services.AddScoped<IListingSecurityItemsRepository, ListingSecurityItemsRepository>();
        services.AddScoped<ICancelationPoliciesTypesRepository, CancelationPoliciesTypesRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ICurrencyRespository, CurrencyRespository>();
        services.AddScoped<IPayTransactionRepository, PayTransactionRepository>();
        services.AddScoped<IListingAdditionalFeeRepository, ListingAdditionalFeeRepository>();
        services.AddScoped<IAssertFeeRepository, AssertFeeRepository>();
        services.AddScoped<IListingRentReviewRepository, ListingRentReviewRepository>();
        services.AddScoped<IReviewQuestionRepository, ReviewQuestionRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
        services.AddScoped<IMessagePredefinedRepository, MessagePredefinedRepository>();
        services.AddScoped<IReasonRefuseBookRepository, ReasonRefusedBookRepository>();
        services.AddScoped<IReasonRefusedPriceCalculationRepository, ReasonRefusedPriceCalculationRepository>();
        services.AddScoped<IBookStatusRepository, BookStatusRepository>();
        #endregion

        return services;
    }
}
