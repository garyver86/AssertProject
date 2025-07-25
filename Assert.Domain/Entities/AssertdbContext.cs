﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assert.Domain.Entities;

public partial class AssertDbContext : DbContext
{
    public AssertDbContext()
    {
    }

    public AssertDbContext(DbContextOptions<AssertDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CitySearchTable> CitySearchTables { get; set; }

    public virtual DbSet<PayCountryConfiguration> PayCountryConfigurations { get; set; }

    public virtual DbSet<PayMethodOfPayment> PayMethodOfPayments { get; set; }

    public virtual DbSet<PayPriceCalculation> PayPriceCalculations { get; set; }

    public virtual DbSet<PayProvider> PayProviders { get; set; }

    public virtual DbSet<PayTransaction> PayTransactions { get; set; }

    public virtual DbSet<TAdditionalSuggestion> TAdditionalSuggestions { get; set; }

    public virtual DbSet<TApprovalPolicyType> TApprovalPolicyTypes { get; set; }

    public virtual DbSet<TAvailabilityBlockType> TAvailabilityBlockTypes { get; set; }

    public virtual DbSet<TBookingPlatform> TBookingPlatforms { get; set; }

    public virtual DbSet<TCalendarBlockType> TCalendarBlockTypes { get; set; }

    public virtual DbSet<TCancelationPolicyType> TCancelationPolicyTypes { get; set; }

    public virtual DbSet<TCity> TCities { get; set; }

    public virtual DbSet<TCountry> TCountries { get; set; }

    public virtual DbSet<TCounty> TCounties { get; set; }

    public virtual DbSet<TCurrency> TCurrencies { get; set; }

    public virtual DbSet<TDiscountTypeForTypePrice> TDiscountTypeForTypePrices { get; set; }

    public virtual DbSet<TDistanceRateType> TDistanceRateTypes { get; set; }

    public virtual DbSet<TErrorParam> TErrorParams { get; set; }

    public virtual DbSet<TExceptionLog> TExceptionLogs { get; set; }

    public virtual DbSet<TFeaturedAspectType> TFeaturedAspectTypes { get; set; }

    public virtual DbSet<TLanguage> TLanguages { get; set; }

    public virtual DbSet<TLocation> TLocations { get; set; }

    public virtual DbSet<TQuickTip> TQuickTips { get; set; }

    public virtual DbSet<TQuickTipType> TQuickTipTypes { get; set; }

    public virtual DbSet<TRentPriceSuggestion> TRentPriceSuggestions { get; set; }

    public virtual DbSet<TResource> TResources { get; set; }

    public virtual DbSet<TSearchLocation> TSearchLocations { get; set; }

    public virtual DbSet<TSpaceType> TSpaceTypes { get; set; }

    public virtual DbSet<TSpecialDatePrice> TSpecialDatePrices { get; set; }

    public virtual DbSet<TState> TStates { get; set; }

    public virtual DbSet<TStayPresenceType> TStayPresenceTypes { get; set; }

    public virtual DbSet<TSuggestGenerate> TSuggestGenerates { get; set; }

    public virtual DbSet<TSystemConfiguration> TSystemConfigurations { get; set; }

    public virtual DbSet<TUnit> TUnits { get; set; }

    public virtual DbSet<TbBook> TbBooks { get; set; }

    public virtual DbSet<TbBookChange> TbBookChanges { get; set; }

    public virtual DbSet<TbBookInsuranceClaim> TbBookInsuranceClaims { get; set; }

    public virtual DbSet<TbBookPayment> TbBookPayments { get; set; }

    public virtual DbSet<TbBookSnapshot> TbBookSnapshots { get; set; }

    public virtual DbSet<TbBookStatus> TbBookStatuses { get; set; }

    public virtual DbSet<TbBookStep> TbBookSteps { get; set; }

    public virtual DbSet<TbBookStepStatus> TbBookStepStatuses { get; set; }

    public virtual DbSet<TbBookStepType> TbBookStepTypes { get; set; }

    public virtual DbSet<TbBookingInsurance> TbBookingInsurances { get; set; }

    public virtual DbSet<TbPaymentStatus> TbPaymentStatuses { get; set; }

    public virtual DbSet<TiIssue> TiIssues { get; set; }

    public virtual DbSet<TiIssueType> TiIssueTypes { get; set; }

    public virtual DbSet<TiStatusIssue> TiStatusIssues { get; set; }

    public virtual DbSet<TimeZone> TimeZones { get; set; }

    public virtual DbSet<TimeZone1> TimeZones1 { get; set; }

    public virtual DbSet<TimeZoneAux> TimeZoneAuxes { get; set; }

    public virtual DbSet<TlAccommodationType> TlAccommodationTypes { get; set; }

    public virtual DbSet<TlAdditionalFee> TlAdditionalFees { get; set; }

    public virtual DbSet<TlCalendarDiscount> TlCalendarDiscounts { get; set; }

    public virtual DbSet<TlCheckInOutPolicy> TlCheckInOutPolicies { get; set; }

    public virtual DbSet<TlExternalReference> TlExternalReferences { get; set; }

    public virtual DbSet<TlGenerateRate> TlGenerateRates { get; set; }

    public virtual DbSet<TlListingAdditionalFee> TlListingAdditionalFees { get; set; }

    public virtual DbSet<TlListingAmenity> TlListingAmenities { get; set; }

    public virtual DbSet<TlListingAvailability> TlListingAvailabilities { get; set; }

    public virtual DbSet<TlListingCalendar> TlListingCalendars { get; set; }

    public virtual DbSet<TlListingCalendarAdditionalFee> TlListingCalendarAdditionalFees { get; set; }

    public virtual DbSet<TlListingDiscountForRate> TlListingDiscountForRates { get; set; }

    public virtual DbSet<TlListingFavorite> TlListingFavorites { get; set; }

    public virtual DbSet<TlListingFavoriteGroup> TlListingFavoriteGroups { get; set; }

    public virtual DbSet<TlListingFeaturedAspect> TlListingFeaturedAspects { get; set; }

    public virtual DbSet<TlListingPhoto> TlListingPhotos { get; set; }

    public virtual DbSet<TlListingPrice> TlListingPrices { get; set; }

    public virtual DbSet<TlListingRent> TlListingRents { get; set; }

    public virtual DbSet<TlListingRentChange> TlListingRentChanges { get; set; }

    public virtual DbSet<TlListingRentRule> TlListingRentRules { get; set; }

    public virtual DbSet<TlListingReview> TlListingReviews { get; set; }

    public virtual DbSet<TlListingSecurityItem> TlListingSecurityItems { get; set; }

    public virtual DbSet<TlListingSpace> TlListingSpaces { get; set; }

    public virtual DbSet<TlListingSpecialDatePrice> TlListingSpecialDatePrices { get; set; }

    public virtual DbSet<TlListingStatus> TlListingStatuses { get; set; }

    public virtual DbSet<TlListingStep> TlListingSteps { get; set; }

    public virtual DbSet<TlListingStepsStatus> TlListingStepsStatuses { get; set; }

    public virtual DbSet<TlListingStepsView> TlListingStepsViews { get; set; }

    public virtual DbSet<TlListingViewHistory> TlListingViewHistories { get; set; }

    public virtual DbSet<TlQuickTypeView> TlQuickTypeViews { get; set; }

    public virtual DbSet<TlStayPresence> TlStayPresences { get; set; }

    public virtual DbSet<TlStepsType> TlStepsTypes { get; set; }

    public virtual DbSet<TlViewType> TlViewTypes { get; set; }

    public virtual DbSet<TmConversation> TmConversations { get; set; }

    public virtual DbSet<TmConversationStatus> TmConversationStatuses { get; set; }

    public virtual DbSet<TmMessage> TmMessages { get; set; }

    public virtual DbSet<TmMessageStatus> TmMessageStatuses { get; set; }

    public virtual DbSet<TmNotification> TmNotifications { get; set; }

    public virtual DbSet<TmTypeMessage> TmTypeMessages { get; set; }

    public virtual DbSet<TpAmenitiesCategory> TpAmenitiesCategories { get; set; }

    public virtual DbSet<TpAmenitiesType> TpAmenitiesTypes { get; set; }

    public virtual DbSet<TpEstimatedRentalIncome> TpEstimatedRentalIncomes { get; set; }

    public virtual DbSet<TpProperty> TpProperties { get; set; }

    public virtual DbSet<TpPropertyAddress> TpPropertyAddresses { get; set; }

    public virtual DbSet<TpPropertySubtype> TpPropertySubtypes { get; set; }

    public virtual DbSet<TpPropertyType> TpPropertyTypes { get; set; }

    public virtual DbSet<TpRuleType> TpRuleTypes { get; set; }

    public virtual DbSet<TpSecurityItemType> TpSecurityItemTypes { get; set; }

    public virtual DbSet<TsInsurance> TsInsurances { get; set; }

    public virtual DbSet<TuAccount> TuAccounts { get; set; }

    public virtual DbSet<TuAccountType> TuAccountTypes { get; set; }

    public virtual DbSet<TuAdditionalProfile> TuAdditionalProfiles { get; set; }

    public virtual DbSet<TuAdditionalProfileLanguage> TuAdditionalProfileLanguages { get; set; }

    public virtual DbSet<TuAdditionalProfileLiveAt> TuAdditionalProfileLiveAts { get; set; }

    public virtual DbSet<TuAddress> TuAddresses { get; set; }

    public virtual DbSet<TuCard> TuCards { get; set; }

    public virtual DbSet<TuDocument> TuDocuments { get; set; }

    public virtual DbSet<TuEmail> TuEmails { get; set; }

    public virtual DbSet<TuEmergencyContact> TuEmergencyContacts { get; set; }

    public virtual DbSet<TuGenderType> TuGenderTypes { get; set; }

    public virtual DbSet<TuPhone> TuPhones { get; set; }

    public virtual DbSet<TuPlatform> TuPlatforms { get; set; }

    public virtual DbSet<TuProfilePhoto> TuProfilePhotos { get; set; }

    public virtual DbSet<TuTitleType> TuTitleTypes { get; set; }

    public virtual DbSet<TuUser> TuUsers { get; set; }

    public virtual DbSet<TuUserListingRent> TuUserListingRents { get; set; }

    public virtual DbSet<TuUserListingRentPayment> TuUserListingRentPayments { get; set; }

    public virtual DbSet<TuUserListingStatus> TuUserListingStatuses { get; set; }

    public virtual DbSet<TuUserLog> TuUserLogs { get; set; }

    public virtual DbSet<TuUserReview> TuUserReviews { get; set; }

    public virtual DbSet<TuUserRole> TuUserRoles { get; set; }

    public virtual DbSet<TuUserStatusType> TuUserStatusTypes { get; set; }

    public virtual DbSet<TuUserType> TuUserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=34.58.179.9;Database=assertDB;uid=assertdb-user;pwd=Fdiah2025%$;Trusted_Connection=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CitySearchTable>(entity =>
        {
            entity.HasKey(e => e.CityId);

            entity.ToTable("CitySearchTable");

            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city_name");
            entity.Property(e => e.CountyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("county_name");
            entity.Property(e => e.StateCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("state_code");
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("state_name");
        });

        modelBuilder.Entity<PayCountryConfiguration>(entity =>
        {
            entity.HasKey(e => e.PaymentConfigId);

            entity.ToTable("Pay_CountryConfiguration");

            entity.Property(e => e.PaymentConfigId).HasColumnName("paymentConfigId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.ConfigurationJson)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("configurationJson");
            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.MethodOfPaymentId).HasColumnName("methodOfPaymentId");
            entity.Property(e => e.Priority)
                .HasDefaultValue(1)
                .HasColumnName("priority");
            entity.Property(e => e.ProviderId).HasColumnName("providerId");

            entity.HasOne(d => d.Country).WithMany(p => p.PayCountryConfigurations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pay_CountryConfiguration_T_Country");

            entity.HasOne(d => d.MethodOfPayment).WithMany(p => p.PayCountryConfigurations)
                .HasForeignKey(d => d.MethodOfPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pay_CountryConfiguration_Pay_MethodOfPayment");

            entity.HasOne(d => d.Provider).WithMany(p => p.PayCountryConfigurations)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pay_CountryConfiguration_Pay_Provider");
        });

        modelBuilder.Entity<PayMethodOfPayment>(entity =>
        {
            entity.HasKey(e => e.MethodOfPaymentId);

            entity.ToTable("Pay_MethodOfPayment");

            entity.Property(e => e.MethodOfPaymentId).HasColumnName("methodOfPaymentId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.MopCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("mopCode");
            entity.Property(e => e.MopDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("mopDescription");
            entity.Property(e => e.MopName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mopName");
            entity.Property(e => e.UrlIcon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("url_icon");
        });

        modelBuilder.Entity<PayPriceCalculation>(entity =>
        {
            entity.HasKey(e => e.PriceCalculationId).HasName("PK__Pay_Pric__8F352993EC496654");

            entity.ToTable("Pay_PriceCalculation");

            entity.HasIndex(e => e.CalculationCode, "IX_CodigoCotizacion");

            entity.HasIndex(e => new { e.CalculationStatue, e.ExpirationDate }, "IX_Estado_FechaExpiracion");

            entity.HasIndex(e => e.BookId, "IX_ReservaID");

            entity.HasIndex(e => e.ListingRentId, "NonClusteredIndex-ListingRentId-20250725");

            entity.HasIndex(e => new { e.InitBook, e.EndBook }, "NonClusteredIndex-bookdates-20250725");

            entity.HasIndex(e => e.CalculationCode, "UQ_CodigoCotizacion").IsUnique();

            entity.Property(e => e.PriceCalculationId).HasColumnName("priceCalculationId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CalculationCode)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("calculationCode");
            entity.Property(e => e.CalculationDetails).HasColumnName("calculationDetails");
            entity.Property(e => e.CalculationStatue)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("calculationStatue");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("currencyCode");
            entity.Property(e => e.EndBook)
                .HasColumnType("datetime")
                .HasColumnName("endBook");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("expirationDate");
            entity.Property(e => e.InitBook)
                .HasColumnType("datetime")
                .HasColumnName("initBook");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.MethodOfPaymentId).HasColumnName("methodOfPaymentId");
            entity.Property(e => e.PaymentProviderId).HasColumnName("paymentProviderId");
            entity.Property(e => e.PaymentTransactionId).HasColumnName("paymentTransactionId");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(256)
                .HasColumnName("userAgent");

            entity.HasOne(d => d.Book).WithMany(p => p.PayPriceCalculations)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_Pay_PriceCalculation_TB_Book");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.PayPriceCalculations)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_Pay_PriceCalculation_TL_ListingRent");

            entity.HasOne(d => d.MethodOfPayment).WithMany(p => p.PayPriceCalculations)
                .HasForeignKey(d => d.MethodOfPaymentId)
                .HasConstraintName("FK_Pay_PriceCalculation_Pay_MethodOfPayment");

            entity.HasOne(d => d.PaymentProvider).WithMany(p => p.PayPriceCalculations)
                .HasForeignKey(d => d.PaymentProviderId)
                .HasConstraintName("FK_Pay_PriceCalculation_Pay_Provider");

            entity.HasOne(d => d.PaymentTransaction).WithMany(p => p.PayPriceCalculations)
                .HasForeignKey(d => d.PaymentTransactionId)
                .HasConstraintName("FK_Pay_PriceCalculation_Pay_Transaction");
        });

        modelBuilder.Entity<PayProvider>(entity =>
        {
            entity.HasKey(e => e.ProviderId);

            entity.ToTable("Pay_Provider");

            entity.Property(e => e.ProviderId).HasColumnName("providerId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.ApiUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("api_url");
            entity.Property(e => e.IntegrationConfiguration)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("integrationConfiguration");
            entity.Property(e => e.ProviderCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("providerCode");
            entity.Property(e => e.ProviderDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("providerDescription");
            entity.Property(e => e.ProviderName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("providerName");
            entity.Property(e => e.ResponseType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("responseType");
        });

        modelBuilder.Entity<PayTransaction>(entity =>
        {
            entity.ToTable("Pay_Transaction");

            entity.Property(e => e.PayTransactionId).HasColumnName("Pay_TransactionId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CupdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("cupdatedAt");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("currencyCode");
            entity.Property(e => e.MethodOfPaymentId).HasColumnName("methodOfPaymentId");
            entity.Property(e => e.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("orderCode");
            entity.Property(e => e.PaymentData)
                .HasMaxLength(3000)
                .IsUnicode(false)
                .HasColumnName("paymentData");
            entity.Property(e => e.PaymentProviderId).HasColumnName("paymentProviderId");
            entity.Property(e => e.Stan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stan");
            entity.Property(e => e.TransactionData)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("transactionData");
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("transactionStatus");
            entity.Property(e => e.TransactionStatusCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("transactionStatusCode");

            entity.HasOne(d => d.Country).WithMany(p => p.PayTransactions)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pay_Transaction_T_Country");

            entity.HasOne(d => d.MethodOfPayment).WithMany(p => p.PayTransactions)
                .HasForeignKey(d => d.MethodOfPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pay_Transaction_Pay_MethodOfPayment");

            entity.HasOne(d => d.PaymentProvider).WithMany(p => p.PayTransactions)
                .HasForeignKey(d => d.PaymentProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pay_Transaction_Pay_Provider");
        });

        modelBuilder.Entity<TAdditionalSuggestion>(entity =>
        {
            entity.HasKey(e => e.AdditionalSuggestionId);

            entity.ToTable("T_AdditionalSuggestion");

            entity.Property(e => e.AdditionalSuggestionId).HasColumnName("additionalSuggestionId");
            entity.Property(e => e.AdditionalMilePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("additionalMilePrice");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.GeneratorAdditionalHourPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("generatorAdditionalHourPrice");
            entity.Property(e => e.GeneratorHourPerDay)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("generatorHourPerDay");
            entity.Property(e => e.MilesPerDay)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("milesPerDay");
            entity.Property(e => e.MinimunRentalPerDay).HasColumnName("minimunRentalPerDay");
        });

        modelBuilder.Entity<TApprovalPolicyType>(entity =>
        {
            entity.HasKey(e => e.ApprovalPolicyTypeId);

            entity.ToTable("T_ApprovalPolicyType");

            entity.Property(e => e.ApprovalPolicyTypeId).HasColumnName("approvalPolicyTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsRecommended).HasColumnName("isRecommended");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TAvailabilityBlockType>(entity =>
        {
            entity.HasKey(e => e.AvailabilityBlockTypeId);

            entity.ToTable("T_AvailabilityBlockType");

            entity.Property(e => e.BlockTypeCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("blockTypeCode");
            entity.Property(e => e.BlockTypeDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("blockTypeDescription");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("color");
        });

        modelBuilder.Entity<TBookingPlatform>(entity =>
        {
            entity.HasKey(e => e.BookingPlatformId);

            entity.ToTable("T_BookingPlatforms");

            entity.Property(e => e.BookingPlatformId).HasColumnName("bookingPlatformId");
            entity.Property(e => e.PlatformCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("platformCode");
            entity.Property(e => e.PlatformIcon)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("platformIcon");
            entity.Property(e => e.PlatformName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("platformName");
        });

        modelBuilder.Entity<TCalendarBlockType>(entity =>
        {
            entity.HasKey(e => e.CalendarBlockTypeId);

            entity.ToTable("T_CalendarBlockType");

            entity.Property(e => e.CalendarBlockTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("calendarBlockTypeId");
            entity.Property(e => e.BlockCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("blockCode");
            entity.Property(e => e.BlockTypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("blockTypeName");
        });

        modelBuilder.Entity<TCancelationPolicyType>(entity =>
        {
            entity.HasKey(e => e.CancelationPolicyTypeId);

            entity.ToTable("T_CancelationPolicyType");

            entity.Property(e => e.CancelationPolicyTypeId).HasColumnName("cancelationPolicyTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Detail1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("detail1");
            entity.Property(e => e.Detail2)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("detail2");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discountPercentage");
            entity.Property(e => e.HoursAfetrBooking)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("hoursAfetrBooking");
            entity.Property(e => e.HoursBeforeCheckIn)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("hoursBeforeCheckIn");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.RefoundUpToDays).HasColumnName("refoundUpToDays");
            entity.Property(e => e.RefoundUpToPercentage).HasColumnName("refoundUpToPercentage");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.WithinDays).HasColumnName("withinDays");
        });

        modelBuilder.Entity<TCity>(entity =>
        {
            entity.HasKey(e => e.CityId);

            entity.ToTable("T_City");

            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.CountyId).HasColumnName("countyId");
            entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.County).WithMany(p => p.TCities)
                .HasForeignKey(d => d.CountyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_City_T_County");
        });

        modelBuilder.Entity<TCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId);

            entity.ToTable("T_Country");

            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IataCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("iataCode");
            entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TCounty>(entity =>
        {
            entity.HasKey(e => e.CountyId);

            entity.ToTable("T_County");

            entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StateId).HasColumnName("stateId");

            entity.HasOne(d => d.State).WithMany(p => p.TCounties)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_T_County_T_State");
        });

        modelBuilder.Entity<TCurrency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId);

            entity.ToTable("T_Currency");

            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("countryCode");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Symbol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("symbol");
        });

        modelBuilder.Entity<TDiscountTypeForTypePrice>(entity =>
        {
            entity.HasKey(e => e.DiscountTypeForTypePriceId);

            entity.ToTable("T_DiscountTypeForTypePrice");

            entity.Property(e => e.DiscountTypeForTypePriceId).HasColumnName("discountTypeForTypePriceId");
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Days).HasColumnName("days");
            entity.Property(e => e.PorcentageSuggest)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentageSuggest");
            entity.Property(e => e.Question)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("question");
            entity.Property(e => e.SuggestDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("suggestDescription");
        });

        modelBuilder.Entity<TDistanceRateType>(entity =>
        {
            entity.HasKey(e => e.DistanceRateTypeId);

            entity.ToTable("T_DistanceRateType");

            entity.Property(e => e.DistanceRateTypeId).HasColumnName("distanceRateTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.NameUnit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nameUnit");
            entity.Property(e => e.SuggestInclude)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("suggestInclude");
            entity.Property(e => e.SuggestIncludeAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("suggestIncludeAmount");
            entity.Property(e => e.SuggestOverage)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("suggestOverage");
            entity.Property(e => e.SuggestOverageAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("suggestOverageAmount");
            entity.Property(e => e.Unit)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<TErrorParam>(entity =>
        {
            entity.HasKey(e => e.ErrorParamId);

            entity.ToTable("T_ErrorParam");

            entity.Property(e => e.ErrorParamId).HasColumnName("errorParamId");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.TechnicalMessage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("technical_message");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<TExceptionLog>(entity =>
        {
            entity.HasKey(e => e.ExceptionLogId);

            entity.ToTable("T_ExceptionLog");

            entity.Property(e => e.ExceptionLogId).HasColumnName("exceptionLogId");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.BrowseInfo)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("browseInfo");
            entity.Property(e => e.DataRequest)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("dataRequest");
            entity.Property(e => e.DateException)
                .HasColumnType("datetime")
                .HasColumnName("dateException");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ipAddress");
            entity.Property(e => e.Message)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.Module)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("module");
            entity.Property(e => e.StackTrace)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("stackTrace");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.TExceptionLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_T_ExceptionLog_T_ExceptionLog");
        });

        modelBuilder.Entity<TFeaturedAspectType>(entity =>
        {
            entity.HasKey(e => e.FeaturedAspectType);

            entity.ToTable("T_FeaturedAspectType");

            entity.Property(e => e.FeaturedAspectType).HasColumnName("featuredAspectType");
            entity.Property(e => e.FeaturedAspectCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("featuredAspectCode");
            entity.Property(e => e.FeaturedAspectName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("featuredAspectName");
            entity.Property(e => e.FeaturedAspectStatus).HasColumnName("featuredAspectStatus");
        });

        modelBuilder.Entity<TLanguage>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__T_Langua__12696A624F5E20A7");

            entity.ToTable("T_Language");

            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Detail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("detail");
        });

        modelBuilder.Entity<TLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId);

            entity.ToTable("T_Location");

            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.Details)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("details");
            entity.Property(e => e.IsTop).HasColumnName("isTop");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UrlImage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("url_image");
        });

        modelBuilder.Entity<TQuickTip>(entity =>
        {
            entity.HasKey(e => e.QuickTipId);

            entity.ToTable("T_QuickTip");

            entity.Property(e => e.QuickTipId).HasColumnName("quickTipId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DisplayElement)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("displayElement");
            entity.Property(e => e.IconLink)
                .IsUnicode(false)
                .HasColumnName("iconLink");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.QuickTipTypeId).HasColumnName("quickTipTypeId");

            entity.HasOne(d => d.QuickTipType).WithMany(p => p.TQuickTips)
                .HasForeignKey(d => d.QuickTipTypeId)
                .HasConstraintName("FK_T_QuickTip_T_QuickTipType");
        });

        modelBuilder.Entity<TQuickTipType>(entity =>
        {
            entity.HasKey(e => e.QuickTipTypeId);

            entity.ToTable("T_QuickTipType");

            entity.Property(e => e.QuickTipTypeId).HasColumnName("quickTipTypeId");
            entity.Property(e => e.DescriptionType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descriptionType");
        });

        modelBuilder.Entity<TRentPriceSuggestion>(entity =>
        {
            entity.HasKey(e => e.RentPriceSuggestionId);

            entity.ToTable("T_RentPriceSuggestion");

            entity.Property(e => e.RentPriceSuggestionId).HasColumnName("rentPriceSuggestionId");
            entity.Property(e => e.AmentiesCount).HasColumnName("amentiesCount");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("currencyCode");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.RentPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("rentPrice");
            entity.Property(e => e.SecurityDepositPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("securityDepositPrice");
            entity.Property(e => e.SizeFrom).HasColumnName("sizeFrom");
            entity.Property(e => e.SizeTo).HasColumnName("sizeTo");
            entity.Property(e => e.YearFrom).HasColumnName("yearFrom");
            entity.Property(e => e.YearTo).HasColumnName("yearTo");
        });

        modelBuilder.Entity<TResource>(entity =>
        {
            entity.ToTable("T_Resource");

            entity.Property(e => e.TResourceId).HasColumnName("T_ResourceId");
            entity.Property(e => e.DateRegister)
                .HasColumnType("datetime")
                .HasColumnName("dateRegister");
            entity.Property(e => e.DateUpdate)
                .HasColumnType("datetime")
                .HasColumnName("dateUpdate");
            entity.Property(e => e.Module)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ResourceCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ResourceName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ResourceOrder).HasColumnName("resourceOrder");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.UserRegisterId).HasColumnName("userRegisterId");
            entity.Property(e => e.UserUpdate).HasColumnName("userUpdate");
        });

        modelBuilder.Entity<TSearchLocation>(entity =>
        {
            entity.HasKey(e => e.SearchValue);

            entity.ToTable("T_SearchLocation");

            entity.Property(e => e.SearchValue)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("search_value");
            entity.Property(e => e.JsonCitiesResult).IsUnicode(false);
            entity.Property(e => e.JsonCountiesResult).IsUnicode(false);
            entity.Property(e => e.JsonCountriesResult).IsUnicode(false);
            entity.Property(e => e.JsonResultLocation).IsUnicode(false);
            entity.Property(e => e.JsonStatesResult).IsUnicode(false);
        });

        modelBuilder.Entity<TSpaceType>(entity =>
        {
            entity.HasKey(e => e.SpaceTypeId);

            entity.ToTable("T_SpaceType");

            entity.Property(e => e.SpaceTypeId).HasColumnName("spaceTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Icon)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TSpecialDatePrice>(entity =>
        {
            entity.HasKey(e => e.SpecialDatePriceId).HasName("PK_TL_SpecialDatePrice");

            entity.ToTable("T_SpecialDatePrice");

            entity.Property(e => e.SpecialDatePriceId).HasColumnName("specialDatePriceId");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.InitDate)
                .HasColumnType("datetime")
                .HasColumnName("initDate");
            entity.Property(e => e.PriceNightly)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("priceNightly");
            entity.Property(e => e.SecurityDepositPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("securityDepositPrice");
            entity.Property(e => e.SpecialDateDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("specialDateDescription");
            entity.Property(e => e.SpecialDateName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("specialDateName");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Currency).WithMany(p => p.TSpecialDatePrices)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_TL_SpecialDatePrice_T_Currency");
        });

        modelBuilder.Entity<TState>(entity =>
        {
            entity.HasKey(e => e.StateId);

            entity.ToTable("T_State");

            entity.Property(e => e.StateId).HasColumnName("stateId");
            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IataCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("iataCode");
            entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Country).WithMany(p => p.TStates)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_State_T_Country");
        });

        modelBuilder.Entity<TStayPresenceType>(entity =>
        {
            entity.HasKey(e => e.StayPrecenseTypeId).HasName("PK__T_StayPr__0464CCD1AE658FCB");

            entity.ToTable("T_StayPresenceType");

            entity.Property(e => e.StayPrecenseTypeId).HasColumnName("stayPrecenseTypeId");
            entity.Property(e => e.StayPrecenseTypeDescription)
                .HasColumnType("text")
                .HasColumnName("stayPrecenseTypeDescription");
            entity.Property(e => e.StayPrecenseTypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stayPrecenseTypeName");
        });

        modelBuilder.Entity<TSuggestGenerate>(entity =>
        {
            entity.HasKey(e => e.SuggestGenerateId);

            entity.ToTable("T_SuggestGenerate");

            entity.Property(e => e.SuggestGenerateId).HasColumnName("suggestGenerateId");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.IncludeHourAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("includeHourAmount");
            entity.Property(e => e.OverChargeAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("overChargeAmount");
            entity.Property(e => e.SuggestIncludeHour)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("suggestIncludeHour");
            entity.Property(e => e.SuggestionOverCharge)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("suggestionOverCharge");
        });

        modelBuilder.Entity<TSystemConfiguration>(entity =>
        {
            entity.HasKey(e => e.SystemConfigurationId);

            entity.ToTable("T_SystemConfiguration");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Value)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("value");
        });

        modelBuilder.Entity<TUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId);

            entity.ToTable("T_Unit");

            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Symbol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("symbol");
        });

        modelBuilder.Entity<TbBook>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK_TB_Booking");

            entity.ToTable("TB_Book");

            entity.Property(e => e.AdditionalInfo)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("additional_info");
            entity.Property(e => e.AmountFees)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("amount_fees");
            entity.Property(e => e.AmountTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount_total");
            entity.Property(e => e.ApprovalDetails)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BookStatusId).HasColumnName("bookStatusId");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.DepositSec)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("depositSec");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.InitDate)
                .HasColumnType("datetime")
                .HasColumnName("initDate");
            entity.Property(e => e.IsApprobal).HasColumnName("isApprobal");
            entity.Property(e => e.IsManualApprobal).HasColumnName("isManualApprobal");
            entity.Property(e => e.LastNameRenter)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lastName_renter");
            entity.Property(e => e.MountPerNight)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("mount_per_night");
            entity.Property(e => e.NameRenter)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name_renter");
            entity.Property(e => e.PaymentCode)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("payment_code");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("paymentId");
            entity.Property(e => e.PickUpLocation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pickUpLocation");
            entity.Property(e => e.Pk)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PK");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.TermsAccepted).HasColumnName("terms_accepted");
            entity.Property(e => e.UserIdRenter).HasColumnName("userId_renter");
            entity.Property(e => e.VggFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("vggFee");
            entity.Property(e => e.VggFeePercent)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("vggFeePercent");

            entity.HasOne(d => d.BookStatus).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.BookStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Book_TB_BookStatus");

            entity.HasOne(d => d.Currency).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Book_T_Currency");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Book_TL_ListingRent");

            entity.HasOne(d => d.UserIdRenterNavigation).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.UserIdRenter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Book_TU_User");
        });

        modelBuilder.Entity<TbBookChange>(entity =>
        {
            entity.HasKey(e => e.BookChangeId);

            entity.ToTable("TB_BookChange");

            entity.Property(e => e.BookChangeId).HasColumnName("bookChangeId");
            entity.Property(e => e.ActionChange)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("actionChange");
            entity.Property(e => e.AdditionalData)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("additionalData");
            entity.Property(e => e.ApplicationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("applicationCode");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.BrowserInfo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("browserInfo");
            entity.Property(e => e.DateTimeChange)
                .HasColumnType("datetime")
                .HasColumnName("dateTimeChange");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ipAddress");
            entity.Property(e => e.IsMobile).HasColumnName("isMobile");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Book).WithMany(p => p.TbBookChanges)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BookChange_TB_Book");
        });

        modelBuilder.Entity<TbBookInsuranceClaim>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("PK__TB_BookI__01BDF9D35C796D7D");

            entity.ToTable("TB_BookInsuranceClaim");

            entity.Property(e => e.ClaimId).HasColumnName("claimId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.ClaimDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("claimDate");
            entity.Property(e => e.ClaimDescription)
                .HasColumnType("text")
                .HasColumnName("claimDescription");
            entity.Property(e => e.ClaimStatusId).HasColumnName("claimStatusId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Booking).WithMany(p => p.TbBookInsuranceClaims)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TB_BookIn__booki__19AACF41");

            entity.HasOne(d => d.User).WithMany(p => p.TbBookInsuranceClaims)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TB_BookIn__userI__1A9EF37A");
        });

        modelBuilder.Entity<TbBookPayment>(entity =>
        {
            entity.HasKey(e => e.BookPaymentId);

            entity.ToTable("TB_BookPayment");

            entity.Property(e => e.BookPaymentId).HasColumnName("bookPaymentId");
            entity.Property(e => e.AdditionalData)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("additionalData");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.DatePayment)
                .HasColumnType("datetime")
                .HasColumnName("datePayment");
            entity.Property(e => e.FormOfPayment)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("formOfPayment");
            entity.Property(e => e.FormOfPaymentId).HasColumnName("formOfPaymentId");
            entity.Property(e => e.PaymentStatusId).HasColumnName("paymentStatusId");
            entity.Property(e => e.TypePayment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("typePayment");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Book).WithMany(p => p.TbBookPayments)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BookPayment_TB_BookPayment");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.TbBookPayments)
                .HasForeignKey(d => d.PaymentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BookPayment_TB_PaymentStatus");
        });

        modelBuilder.Entity<TbBookSnapshot>(entity =>
        {
            entity.HasKey(e => e.BookSnapshotId);

            entity.ToTable("TB_BookSnapshot");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.AmountDiscount)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Amount_discount");
            entity.Property(e => e.AmountNightly).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.AmountNightlyDiscount)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("AmountNightly_discount");
            entity.Property(e => e.BookDate).HasColumnType("datetime");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DepositSegure).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.DiscountDetails)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DiscountRate).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FopDetails)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("fop_details");
            entity.Property(e => e.FormOfPayment)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.GeneratorDescription)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ListingName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Miles).HasColumnName("miles");
            entity.Property(e => e.MilesPolicyDescription)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Model)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Policies)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PropertyId).HasColumnName("propertyId");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.SubtotalAmount).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.SubtotalAmountDiscount)
                .HasColumnType("decimal(18, 5)")
                .HasColumnName("SubtotalAmount_Discount");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.TotalAmountDiscount)
                .HasColumnType("decimal(18, 5)")
                .HasColumnName("TotalAmount_Discount");
            entity.Property(e => e.TypeApprovalDescription)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.Book).WithMany(p => p.TbBookSnapshots)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BookSnapshot_TB_Book");
        });

        modelBuilder.Entity<TbBookStatus>(entity =>
        {
            entity.HasKey(e => e.BookStatusId);

            entity.ToTable("TB_BookStatus");

            entity.Property(e => e.BookStatusId).HasColumnName("bookStatusId");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TbBookStep>(entity =>
        {
            entity.HasKey(e => e.BookStepId);

            entity.ToTable("TB_BookStep");

            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.IsEnded).HasColumnName("isEnded");

            entity.HasOne(d => d.Book).WithMany(p => p.TbBookSteps)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BookStep_TB_Book");

            entity.HasOne(d => d.BookStepStatus).WithMany(p => p.TbBookSteps)
                .HasForeignKey(d => d.BookStepStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BookStep_TB_BookStepStatus");

            entity.HasOne(d => d.BookStepTypeView).WithMany(p => p.TbBookSteps)
                .HasForeignKey(d => d.BookStepTypeViewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BookStep_TB_BookStepType");
        });

        modelBuilder.Entity<TbBookStepStatus>(entity =>
        {
            entity.HasKey(e => e.BookStepStatusId);

            entity.ToTable("TB_BookStepStatus");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbBookStepType>(entity =>
        {
            entity.HasKey(e => e.BookStepTypeId);

            entity.ToTable("TB_BookStepType");

            entity.Property(e => e.BookStepTypeId).HasColumnName("BookStepTypeID");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.DataView)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PreviousStepCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TableData)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbBookingInsurance>(entity =>
        {
            entity.HasKey(e => e.BookingInsuranceId).HasName("PK__TB_Booki__C2134D3F1AD8AD78");

            entity.ToTable("TB_BookingInsurance");

            entity.Property(e => e.BookingInsuranceId).HasColumnName("bookingInsuranceId");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.InsuranceId).HasColumnName("insuranceId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Booking).WithMany(p => p.TbBookingInsurances)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TB_Bookin__booki__17C286CF");

            entity.HasOne(d => d.Insurance).WithMany(p => p.TbBookingInsurances)
                .HasForeignKey(d => d.InsuranceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TB_Bookin__insur__18B6AB08");
        });

        modelBuilder.Entity<TbPaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusId);

            entity.ToTable("TB_PaymentStatus");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TiIssue>(entity =>
        {
            entity.HasKey(e => e.IssueId).HasName("PK__TI_Issue__749E806C0907CC6B");

            entity.ToTable("TI_Issues");

            entity.Property(e => e.IssueId).HasColumnName("issueId");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.DescriptionIssue)
                .HasColumnType("text")
                .HasColumnName("descriptionIssue");
            entity.Property(e => e.IssueTypeId).HasColumnName("issueTypeId");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.RelatedUserId).HasColumnName("related_userId");
            entity.Property(e => e.ReportedByUserId).HasColumnName("reportedBy_userId");
            entity.Property(e => e.ReportedDate)
                .HasColumnType("datetime")
                .HasColumnName("reported_date");
            entity.Property(e => e.ResolvedDate)
                .HasColumnType("datetime")
                .HasColumnName("resolved_date");
            entity.Property(e => e.StatusIssueId).HasColumnName("statusIssueId");

            entity.HasOne(d => d.Booking).WithMany(p => p.TiIssues)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_TI_Issues_TB_Book");

            entity.HasOne(d => d.IssueType).WithMany(p => p.TiIssues)
                .HasForeignKey(d => d.IssueTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TI_Issues_TI_IssueType");

            entity.HasOne(d => d.RelatedUser).WithMany(p => p.TiIssueRelatedUsers)
                .HasForeignKey(d => d.RelatedUserId)
                .HasConstraintName("FK__TI_Issues__relat__22401542");

            entity.HasOne(d => d.ReportedByUser).WithMany(p => p.TiIssueReportedByUsers)
                .HasForeignKey(d => d.ReportedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TI_Issues__repor__2334397B");

            entity.HasOne(d => d.StatusIssue).WithMany(p => p.TiIssues)
                .HasForeignKey(d => d.StatusIssueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TI_Issues__prope__0D99FE17");
        });

        modelBuilder.Entity<TiIssueType>(entity =>
        {
            entity.HasKey(e => e.IssueTypeId);

            entity.ToTable("TI_IssueType");

            entity.Property(e => e.IssueTypeId).HasColumnName("issueTypeId");
            entity.Property(e => e.IssueCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IssueName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiStatusIssue>(entity =>
        {
            entity.HasKey(e => e.StatusIssueId);

            entity.ToTable("TI_StatusIssue");

            entity.Property(e => e.StatusIssueId).HasColumnName("statusIssueId");
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("statusDescription");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("statusName");
        });

        modelBuilder.Entity<TimeZone>(entity =>
        {
            entity.HasKey(e => e.TimeZone1);

            entity.ToTable("time_zone");

            entity.Property(e => e.TimeZone1).HasColumnName("time_zone");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("abbreviation");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("country_code");
            entity.Property(e => e.Dst)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dst");
            entity.Property(e => e.GmtOffset).HasColumnName("gmt_offset");
            entity.Property(e => e.TimeStart).HasColumnName("time_start");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("zone_name");
        });

        modelBuilder.Entity<TimeZone1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__timezone__3213E83FC0452791");

            entity.ToTable("TimeZone");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("abbreviation");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("country_code");
            entity.Property(e => e.Diff)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("diff");
            entity.Property(e => e.Dst).HasColumnName("dst");
            entity.Property(e => e.GmtOffset).HasColumnName("gmt_offset");
            entity.Property(e => e.TimeStart).HasColumnName("time_start");
            entity.Property(e => e.TimezoneId)
                .HasMaxLength(155)
                .IsUnicode(false)
                .HasColumnName("timezone_id");
            entity.Property(e => e.TimezoneLabel)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("timezone_label");
            entity.Property(e => e.TimezoneName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("timezone_name");
        });

        modelBuilder.Entity<TimeZoneAux>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TimeZone_AUX");

            entity.Property(e => e.Abbreviation)
                .HasMaxLength(50)
                .HasColumnName("abbreviation");
            entity.Property(e => e.ConeName)
                .HasMaxLength(50)
                .HasColumnName("cone_name");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .HasColumnName("country_code");
            entity.Property(e => e.Dst)
                .HasMaxLength(50)
                .HasColumnName("dst");
            entity.Property(e => e.GmtOffset)
                .HasMaxLength(50)
                .HasColumnName("gmt_offset");
            entity.Property(e => e.TimeStart).HasColumnName("time_start");
        });

        modelBuilder.Entity<TlAccommodationType>(entity =>
        {
            entity.HasKey(e => e.AccommodationTypeId).HasName("PK__TL_Accom__7380C37AEACCEFD6");

            entity.ToTable("TL_AccommodationType");

            entity.Property(e => e.AccommodationTypeId).HasColumnName("accommodationTypeId");
            entity.Property(e => e.AccommodationDescription)
                .HasColumnType("text")
                .HasColumnName("accommodationDescription");
            entity.Property(e => e.AccommodationIcon)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("accommodationIcon");
            entity.Property(e => e.AccommodationName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("accommodationName");
            entity.Property(e => e.AccomodationCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("accomodationCode");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TlAdditionalFee>(entity =>
        {
            entity.HasKey(e => e.AdditionalFeeId).HasName("PK__TL_Addit__C33EEAE5A4BC0F1A");

            entity.ToTable("TL_AdditionalFees");

            entity.Property(e => e.AdditionalFeeId).HasColumnName("additionalFeeId");
            entity.Property(e => e.CalculationType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("calculationType");
            entity.Property(e => e.DeeDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("deeDescription");
            entity.Property(e => e.FeeCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("feeCode");
            entity.Property(e => e.FeeValue)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("feeValue");
        });

        modelBuilder.Entity<TlCalendarDiscount>(entity =>
        {
            entity.HasKey(e => e.CalendarDiscount);

            entity.ToTable("TL_CalendarDiscount");

            entity.Property(e => e.CalendarDiscount).HasColumnName("calendarDiscount");
            entity.Property(e => e.CalendarId).HasColumnName("calendarId");
            entity.Property(e => e.DiscountCalculated)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("discountCalculated");
            entity.Property(e => e.DiscountTypeForTypePriceId).HasColumnName("discountTypeForTypePriceId");
            entity.Property(e => e.IsDiscount).HasColumnName("isDiscount");
            entity.Property(e => e.Porcentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentage");

            entity.HasOne(d => d.Calendar).WithMany(p => p.TlCalendarDiscounts)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_CalendarDiscount_TL_ListingCalendar");

            entity.HasOne(d => d.DiscountTypeForTypePrice).WithMany(p => p.TlCalendarDiscounts)
                .HasForeignKey(d => d.DiscountTypeForTypePriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_Discount_TL_CalendarDiscount");
        });

        modelBuilder.Entity<TlCheckInOutPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__TL_Check__78E3A922013CDAC6");

            entity.ToTable("TL_CheckInOutPolicies");

            entity.Property(e => e.PolicyId).HasColumnName("policyId");
            entity.Property(e => e.CheckInTime).HasColumnName("checkInTime");
            entity.Property(e => e.CheckOutTime).HasColumnName("checkOutTime");
            entity.Property(e => e.FlexibleCheckIn)
                .HasDefaultValue(false)
                .HasColumnName("flexibleCheckIn");
            entity.Property(e => e.FlexibleCheckOut)
                .HasDefaultValue(false)
                .HasColumnName("flexibleCheckOut");
            entity.Property(e => e.Instructions)
                .HasColumnType("text")
                .HasColumnName("instructions");
            entity.Property(e => e.LateCheckInFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("lateCheckInFee");
            entity.Property(e => e.LateCheckOutFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("lateCheckOutFee");
            entity.Property(e => e.ListingRentid).HasColumnName("listingRentid");
            entity.Property(e => e.MaxCheckInTime).HasColumnName("maxCheckInTime");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlCheckInOutPolicies)
                .HasForeignKey(d => d.ListingRentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TL_CheckI__listi__004002F9");
        });

        modelBuilder.Entity<TlExternalReference>(entity =>
        {
            entity.HasKey(e => e.ExternalReferenceId);

            entity.ToTable("TL_ExternalReference");

            entity.Property(e => e.ExternalReferenceId).HasColumnName("externalReferenceId");
            entity.Property(e => e.BookingPlatformId).HasColumnName("bookingPlatformId");
            entity.Property(e => e.ExternalIdentifier)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("externalIdentifier");
            entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");

            entity.HasOne(d => d.BookingPlatform).WithMany(p => p.TlExternalReferences)
                .HasForeignKey(d => d.BookingPlatformId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ExternalReference_T_BookingPlatforms");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlExternalReferences)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ExternalReference_TL_ListingRent");
        });

        modelBuilder.Entity<TlGenerateRate>(entity =>
        {
            entity.HasKey(e => e.GenerateRateId);

            entity.ToTable("TL_GenerateRate");

            entity.Property(e => e.GenerateRateId).HasColumnName("generateRateId");
            entity.Property(e => e.IncludeHour).HasColumnName("includeHour");
            entity.Property(e => e.IsGenerate).HasColumnName("isGenerate");
            entity.Property(e => e.IsGenerateAllowUnlimited).HasColumnName("isGenerateAllowUnlimited");
            entity.Property(e => e.OverageCharge)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("overageCharge");
            entity.Property(e => e.SuggestGenerateId).HasColumnName("suggestGenerateId");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlGenerateRates)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TL_GenerateRate_TL_ListingRent");

            entity.HasOne(d => d.SuggestGenerate).WithMany(p => p.TlGenerateRates)
                .HasForeignKey(d => d.SuggestGenerateId)
                .HasConstraintName("FK_TL_GenerateRate_T_SuggestGenerate");
        });

        modelBuilder.Entity<TlListingAdditionalFee>(entity =>
        {
            entity.HasKey(e => e.ListingAdditionalFeeId);

            entity.ToTable("TL_ListingAdditionalFee");

            entity.Property(e => e.ListingAdditionalFeeId).HasColumnName("listingAdditionalFeeId");
            entity.Property(e => e.AdditionalFeeId).HasColumnName("additionalFeeId");
            entity.Property(e => e.AmountFee)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("amountFee");
            entity.Property(e => e.IsPercent).HasColumnName("isPercent");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");

            entity.HasOne(d => d.AdditionalFee).WithMany(p => p.TlListingAdditionalFees)
                .HasForeignKey(d => d.AdditionalFeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingAdditionalFee_TL_AdditionalFees");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingAdditionalFees)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingAdditionalFee_TL_ListingRent");
        });

        modelBuilder.Entity<TlListingAmenity>(entity =>
        {
            entity.HasKey(e => e.ListingAmenitiesId);

            entity.ToTable("TL_ListingAmenities");

            entity.Property(e => e.AmenitiesTypeId).HasColumnName("amenitiesTypeId");
            entity.Property(e => e.IsPremium).HasColumnName("isPremium");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.ValueString)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("valueString");

            entity.HasOne(d => d.AmenitiesType).WithMany(p => p.TlListingAmenities)
                .HasForeignKey(d => d.AmenitiesTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingAmenities_TP_AmenitiesType");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingAmenities)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingAmenities_TL_ListingRent");
        });

        modelBuilder.Entity<TlListingAvailability>(entity =>
        {
            entity.HasKey(e => e.ListingAvailabilityId);

            entity.ToTable("TL_ListingAvailability");

            entity.Property(e => e.ListingAvailabilityId).HasColumnName("listingAvailabilityId");
            entity.Property(e => e.BlockTypeId).HasColumnName("blockTypeId");
            entity.Property(e => e.BlockedFrom).HasColumnName("blockedFrom");
            entity.Property(e => e.BlockedFromDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("blockedFromDescription");
            entity.Property(e => e.BlockedTo).HasColumnName("blockedTo");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.RenterName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UserIdRenter).HasColumnName("userId_renter");

            entity.HasOne(d => d.BlockType).WithMany(p => p.TlListingAvailabilities)
                .HasForeignKey(d => d.BlockTypeId)
                .HasConstraintName("FK_TL_ListingAvailability_T_AvailabilityBlockType");

            entity.HasOne(d => d.Book).WithMany(p => p.TlListingAvailabilities)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_TL_ListingAvailability_TB_Book");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingAvailabilities)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TL_ListingAvailability_TL_ListingRent");
        });

        modelBuilder.Entity<TlListingCalendar>(entity =>
        {
            entity.HasKey(e => e.CalendarId).HasName("PK__TL_Listi__EE5496F6EE7EEB5C");

            entity.ToTable("TL_ListingCalendar");

            entity.Property(e => e.CalendarId).HasColumnName("calendarId");
            entity.Property(e => e.AvailabilityWindowMonth).HasColumnName("availabilityWindowMonth");
            entity.Property(e => e.BlockReason)
                .HasMaxLength(100)
                .HasColumnName("blockReason");
            entity.Property(e => e.BlockType).HasColumnName("blockType");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CheckInDays)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("checkInDays");
            entity.Property(e => e.CheckOutDays)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("checkOutDays");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.ListingrentId).HasColumnName("listingrentId");
            entity.Property(e => e.MaximumStay).HasColumnName("maximumStay");
            entity.Property(e => e.MinimumNotice).HasColumnName("minimumNotice");
            entity.Property(e => e.MinimumNoticeHour).HasColumnName("minimumNoticeHour");
            entity.Property(e => e.MinimumStay).HasColumnName("minimumStay");
            entity.Property(e => e.PreparationDays).HasColumnName("preparationDays");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.BlockTypeNavigation).WithMany(p => p.TlListingCalendars)
                .HasForeignKey(d => d.BlockType)
                .HasConstraintName("FK_TL_ListingCalendar_T_CalendarBlockType");

            entity.HasOne(d => d.Book).WithMany(p => p.TlListingCalendars)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_TL_ListingCalendar_TB_Book");

            entity.HasOne(d => d.Listingrent).WithMany(p => p.TlListingCalendars)
                .HasForeignKey(d => d.ListingrentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingCalendar_TL_ListingRent");
        });

        modelBuilder.Entity<TlListingCalendarAdditionalFee>(entity =>
        {
            entity.HasKey(e => e.ListingCalendarAdditionalFeeId);

            entity.ToTable("TL_ListingCalendarAdditionalFee");

            entity.Property(e => e.ListingCalendarAdditionalFeeId).HasColumnName("listingCalendarAdditionalFeeId");
            entity.Property(e => e.AdditionalFeeId).HasColumnName("additionalFeeId");
            entity.Property(e => e.AmountFee)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("amountFee");
            entity.Property(e => e.CalendarId).HasColumnName("calendarId");
            entity.Property(e => e.IsPercent).HasColumnName("isPercent");

            entity.HasOne(d => d.AdditionalFee).WithMany(p => p.TlListingCalendarAdditionalFees)
                .HasForeignKey(d => d.AdditionalFeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingCalendarAdditionalFee_TL_AdditionalFees");

            entity.HasOne(d => d.Calendar).WithMany(p => p.TlListingCalendarAdditionalFees)
                .HasForeignKey(d => d.CalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingCalendarAdditionalFee_TL_ListingCalendar");
        });

        modelBuilder.Entity<TlListingDiscountForRate>(entity =>
        {
            entity.HasKey(e => e.ListingDiscountForRate);

            entity.ToTable("TL_ListingDiscountForRate");

            entity.Property(e => e.ListingDiscountForRate).HasColumnName("listingDiscountForRate");
            entity.Property(e => e.DiscountCalculated)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("discountCalculated");
            entity.Property(e => e.DiscountTypeForTypePriceId).HasColumnName("discountTypeForTypePriceId");
            entity.Property(e => e.IsDiscount).HasColumnName("isDiscount");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.Porcentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentage");

            entity.HasOne(d => d.DiscountTypeForTypePrice).WithMany(p => p.TlListingDiscountForRates)
                .HasForeignKey(d => d.DiscountTypeForTypePriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingDiscountForRate_T_DiscountTypeForTypePrice");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingDiscountForRates)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingDiscountForRate_TL_ListingDiscountForRate");
        });

        modelBuilder.Entity<TlListingFavorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteListingId);

            entity.ToTable("TL_ListingFavorite");

            entity.Property(e => e.FavoriteListingId).HasColumnName("favoriteListingId");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.FavoriteGroupId).HasColumnName("favoriteGroupId");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.FavoriteGroup).WithMany(p => p.TlListingFavorites)
                .HasForeignKey(d => d.FavoriteGroupId)
                .HasConstraintName("FK_TL_ListingFavorite_TL_ListingFavoriteGroup");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingFavorites)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingFavorite_TL_ListingRent");

            entity.HasOne(d => d.User).WithMany(p => p.TlListingFavorites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingFavorite_TU_User");
        });

        modelBuilder.Entity<TlListingFavoriteGroup>(entity =>
        {
            entity.HasKey(e => e.FavoriteGroupListingId);

            entity.ToTable("TL_ListingFavoriteGroup");

            entity.Property(e => e.FavoriteGroupListingId).HasColumnName("favoriteGroupListingId");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.FavoriteGroupName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("favoriteGroupName");
            entity.Property(e => e.GroupStatus).HasColumnName("groupStatus");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.TlListingFavoriteGroups)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingFavoriteGroup_TU_User");
        });

        modelBuilder.Entity<TlListingFeaturedAspect>(entity =>
        {
            entity.HasKey(e => e.ListingFeaturedAspectId);

            entity.ToTable("TL_ListingFeaturedAspect");

            entity.Property(e => e.ListingFeaturedAspectId).HasColumnName("listingFeaturedAspectId");
            entity.Property(e => e.FeaturedAspectValue)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("featuredAspectValue");
            entity.Property(e => e.FeaturesAspectTypeId).HasColumnName("featuresAspectTypeId");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");

            entity.HasOne(d => d.FeaturesAspectType).WithMany(p => p.TlListingFeaturedAspects)
                .HasForeignKey(d => d.FeaturesAspectTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingFeaturedAspect_T_FeaturedAspectType");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingFeaturedAspects)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingFeaturedAspect_TL_ListingRent");
        });

        modelBuilder.Entity<TlListingPhoto>(entity =>
        {
            entity.HasKey(e => e.ListingPhotoId);

            entity.ToTable("TL_ListingPhoto");

            entity.Property(e => e.ListingPhotoId).HasColumnName("listingPhotoId");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsOutstanding).HasColumnName("isOutstanding");
            entity.Property(e => e.IsPrincipal).HasColumnName("isPrincipal");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhotoLink)
                .IsUnicode(false)
                .HasColumnName("photoLink");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.SpaceTypeId).HasColumnName("spaceTypeId");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingPhotos)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TL_ListingPhoto_TL_ListingRent");

            entity.HasOne(d => d.SpaceType).WithMany(p => p.TlListingPhotos)
                .HasForeignKey(d => d.SpaceTypeId)
                .HasConstraintName("FK_TL_ListingPhoto_T_SpaceType");
        });

        modelBuilder.Entity<TlListingPrice>(entity =>
        {
            entity.HasKey(e => e.ListingPriceId);

            entity.ToTable("TL_ListingPrice");

            entity.Property(e => e.ListingPriceId).HasColumnName("listingPriceId");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.ListingPriceOfferId).HasColumnName("listingPriceOfferId");
            entity.Property(e => e.PriceNightly)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("priceNightly");
            entity.Property(e => e.SecurityDepositPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("securityDepositPrice");
            entity.Property(e => e.TimeUnitId).HasColumnName("timeUnitId");
            entity.Property(e => e.WeekendNightlyPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("weekendNightlyPrice");

            entity.HasOne(d => d.Currency).WithMany(p => p.TlListingPrices)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_TL_ListingPrice_T_Currency");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingPrices)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingPrice_TL_ListingRent");
        });

        modelBuilder.Entity<TlListingRent>(entity =>
        {
            entity.HasKey(e => e.ListingRentId).HasName("PK_ListingRent");

            entity.ToTable("TL_ListingRent");

            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.AccomodationTypeId).HasColumnName("accomodationTypeId");
            entity.Property(e => e.AllDoorsLocked).HasColumnName("allDoorsLocked");
            entity.Property(e => e.ApprovalPolicyTypeId).HasColumnName("approvalPolicyTypeId");
            entity.Property(e => e.ApprovalRequestDays).HasColumnName("approvalRequestDays");
            entity.Property(e => e.AvailabilityWindowMonth).HasColumnName("availabilityWindowMonth");
            entity.Property(e => e.AvgReviews)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("avgReviews");
            entity.Property(e => e.Bathrooms).HasColumnName("bathrooms");
            entity.Property(e => e.Bedrooms).HasColumnName("bedrooms");
            entity.Property(e => e.Beds).HasColumnName("beds");
            entity.Property(e => e.CancelationPolicyTypeId).HasColumnName("cancelationPolicyTypeId");
            entity.Property(e => e.CheckInDays)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("checkInDays");
            entity.Property(e => e.CheckOutDays)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("checkOutDays");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ExternalCameras).HasColumnName("externalCameras");
            entity.Property(e => e.ListingRentConfirmationDate)
                .HasColumnType("datetime")
                .HasColumnName("listingRentConfirmationDate");
            entity.Property(e => e.ListingStatusId).HasColumnName("listingStatusId");
            entity.Property(e => e.MaxGuests).HasColumnName("maxGuests");
            entity.Property(e => e.MaximumStay).HasColumnName("maximumStay");
            entity.Property(e => e.MinimumNotice).HasColumnName("minimumNotice");
            entity.Property(e => e.MinimumNoticeHour).HasColumnName("minimumNoticeHour");
            entity.Property(e => e.MinimunRentalPerDay)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("minimunRentalPerDay");
            entity.Property(e => e.MinimunStay).HasColumnName("minimunStay");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NoiseDesibelesMonitor).HasColumnName("noiseDesibelesMonitor");
            entity.Property(e => e.OwnerUserId).HasColumnName("ownerUserId");
            entity.Property(e => e.PreparationDays).HasColumnName("preparationDays");
            entity.Property(e => e.PresenceOfWeapons).HasColumnName("presenceOfWeapons");
            entity.Property(e => e.PrivateBathroom).HasColumnName("privateBathroom");
            entity.Property(e => e.PrivateBathroomLodging).HasColumnName("privateBathroomLodging");
            entity.Property(e => e.SharedBathroom).HasColumnName("sharedBathroom");
            entity.Property(e => e.StepsCount).HasColumnName("stepsCount");

            entity.HasOne(d => d.AccomodationType).WithMany(p => p.TlListingRents)
                .HasForeignKey(d => d.AccomodationTypeId)
                .HasConstraintName("FK_TL_ListingRent_TL_AccommodationType");

            entity.HasOne(d => d.ApprovalPolicyType).WithMany(p => p.TlListingRents)
                .HasForeignKey(d => d.ApprovalPolicyTypeId)
                .HasConstraintName("FK_TL_ListingRent_T_ApprovalPolicyType");

            entity.HasOne(d => d.CancelationPolicyType).WithMany(p => p.TlListingRents)
                .HasForeignKey(d => d.CancelationPolicyTypeId)
                .HasConstraintName("FK_TL_ListingRent_T_CancelationPolicyType");

            entity.HasOne(d => d.ListingStatus).WithMany(p => p.TlListingRents)
                .HasForeignKey(d => d.ListingStatusId)
                .HasConstraintName("FK_TL_ListingRent_TL_ListingStatus");

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.TlListingRents)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingRent_TU_User");
        });

        modelBuilder.Entity<TlListingRentChange>(entity =>
        {
            entity.HasKey(e => e.ListingRentChangesId);

            entity.ToTable("TL_ListingRentChanges");

            entity.Property(e => e.ActionChange)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AdditionalData)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ApplicationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BrowserInfoInfo)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.DateTimeChange).HasColumnType("datetime");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingRentChanges)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingRentChanges_TL_ListingRent");
        });

        modelBuilder.Entity<TlListingRentRule>(entity =>
        {
            entity.HasKey(e => e.ListingRulesId);

            entity.ToTable("TL_ListingRentRules");

            entity.Property(e => e.ListingRulesId).HasColumnName("listingRulesId");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.RuleTypeId).HasColumnName("ruleTypeId");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.ValueString)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("valueString");

            entity.HasOne(d => d.Listing).WithMany(p => p.TlListingRentRules)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("FK_TL_ListingRentRules_TL_ListingRent");

            entity.HasOne(d => d.RuleType).WithMany(p => p.TlListingRentRules)
                .HasForeignKey(d => d.RuleTypeId)
                .HasConstraintName("FK_TL_ListingRentRules_TP_RuleType");
        });

        modelBuilder.Entity<TlListingReview>(entity =>
        {
            entity.HasKey(e => e.ListingReviewId);

            entity.ToTable("TL_ListingReview");

            entity.Property(e => e.ListingReviewId).HasColumnName("listingReviewId");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Calification).HasColumnName("calification");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.DateTimeReview)
                .HasColumnType("datetime")
                .HasColumnName("dateTimeReview");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Book).WithMany(p => p.TlListingReviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_TL_ListingReview_TB_Book");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingReviews)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingReview_TL_ListingRent");

            entity.HasOne(d => d.User).WithMany(p => p.TlListingReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingReview_TU_User");
        });

        modelBuilder.Entity<TlListingSecurityItem>(entity =>
        {
            entity.HasKey(e => e.ListingSecurityItemId);

            entity.ToTable("TL_ListingSecurityItem");

            entity.Property(e => e.SecurityItemTypeId).HasColumnName("securityItemTypeId");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.ValueString)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("valueString");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingSecurityItems)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TL_ListingSecurityItem_TL_ListingRent");

            entity.HasOne(d => d.SecurityItemType).WithMany(p => p.TlListingSecurityItems)
                .HasForeignKey(d => d.SecurityItemTypeId)
                .HasConstraintName("FK_TL_ListingSecurityItem_TP_SecurityItemType");
        });

        modelBuilder.Entity<TlListingSpace>(entity =>
        {
            entity.HasKey(e => e.ListingSpaceId);

            entity.ToTable("TL_ListingSpace");

            entity.Property(e => e.ListingSpaceId).HasColumnName("listingSpaceId");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.SpaceTypeId).HasColumnName("spaceTypeId");
            entity.Property(e => e.Value1).HasColumnName("value1");

            entity.HasOne(d => d.Listing).WithMany(p => p.TlListingSpaces)
                .HasForeignKey(d => d.ListingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingSpace_TL_ListingRent");

            entity.HasOne(d => d.SpaceType).WithMany(p => p.TlListingSpaces)
                .HasForeignKey(d => d.SpaceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingSpace_T_SpaceType");
        });

        modelBuilder.Entity<TlListingSpecialDatePrice>(entity =>
        {
            entity.HasKey(e => e.SpecialDatePriceId);

            entity.ToTable("TL_ListingSpecialDatePrice");

            entity.Property(e => e.SpecialDatePriceId).HasColumnName("specialDatePriceId");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.InitDate)
                .HasColumnType("datetime")
                .HasColumnName("initDate");
            entity.Property(e => e.PriceNightly)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("priceNightly");
            entity.Property(e => e.SecurityDepositPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("securityDepositPrice");
            entity.Property(e => e.SpecialDateDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("specialDateDescription");
            entity.Property(e => e.SpecialDateName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("specialDateName");
            entity.Property(e => e.SpecialDatePriceReferenceId).HasColumnName("specialDatePriceReferenceId");

            entity.HasOne(d => d.Currency).WithMany(p => p.TlListingSpecialDatePrices)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_TL_ListingSpecialDatePrice_T_Currency");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingSpecialDatePrices)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TL_ListingSpecialDatePrice_TL_ListingRent");

            entity.HasOne(d => d.SpecialDatePriceReference).WithMany(p => p.TlListingSpecialDatePrices)
                .HasForeignKey(d => d.SpecialDatePriceReferenceId)
                .HasConstraintName("FK_TL_ListingSpecialDatePrice_T_SpecialDatePrice");
        });

        modelBuilder.Entity<TlListingStatus>(entity =>
        {
            entity.HasKey(e => e.ListingStatusId).HasName("PK_ListingStatus");

            entity.ToTable("TL_ListingStatus");

            entity.Property(e => e.ListingStatusId).HasColumnName("listingStatusId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TlListingStep>(entity =>
        {
            entity.HasKey(e => e.ListingStepsId);

            entity.ToTable("TL_ListingSteps");

            entity.Property(e => e.ListingStepsId).HasColumnName("listingStepsId");
            entity.Property(e => e.AditionalDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("aditionalDetail");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.ListingStepsStatusId).HasColumnName("listingStepsStatusId");
            entity.Property(e => e.StepsTypeId).HasColumnName("stepsTypeId");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingSteps)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TL_ListingSteps_TL_ListingRent");

            entity.HasOne(d => d.ListingStepsStatus).WithMany(p => p.TlListingSteps)
                .HasForeignKey(d => d.ListingStepsStatusId)
                .HasConstraintName("FK_TL_ListingSteps_TL_ListingStepsStatus");

            entity.HasOne(d => d.StepsType).WithMany(p => p.TlListingSteps)
                .HasForeignKey(d => d.StepsTypeId)
                .HasConstraintName("FK_TL_ListingSteps_TL_StepsType");
        });

        modelBuilder.Entity<TlListingStepsStatus>(entity =>
        {
            entity.HasKey(e => e.ListingStepsStatusId);

            entity.ToTable("TL_ListingStepsStatus");

            entity.Property(e => e.ListingStepsStatusId).HasColumnName("listingStepsStatusId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TlListingStepsView>(entity =>
        {
            entity.HasKey(e => e.ListngStepsViewId);

            entity.ToTable("TL_ListingStepsView");

            entity.Property(e => e.ListngStepsViewId).HasColumnName("listngStepsViewId");
            entity.Property(e => e.IsEnded).HasColumnName("isEnded");
            entity.Property(e => e.ListingStepsId).HasColumnName("listingStepsId");
            entity.Property(e => e.Mode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("mode");
            entity.Property(e => e.ViewTypeId).HasColumnName("viewTypeId");

            entity.HasOne(d => d.ListingSteps).WithMany(p => p.TlListingStepsViews)
                .HasForeignKey(d => d.ListingStepsId)
                .HasConstraintName("FK_TL_ListingStepsView_TL_ListingSteps");

            entity.HasOne(d => d.ViewType).WithMany(p => p.TlListingStepsViews)
                .HasForeignKey(d => d.ViewTypeId)
                .HasConstraintName("FK_TL_ListingStepsView_TL_ViewType");
        });

        modelBuilder.Entity<TlListingViewHistory>(entity =>
        {
            entity.HasKey(e => e.ListingViewHitoryId);

            entity.ToTable("TL_ListingViewHistory");

            entity.HasIndex(e => e.ListingRentId, "NonClusteredIndex-ListingRentId-20250705-213930");

            entity.HasIndex(e => e.UserId, "NonClusteredIndex-UserId-20250705-213858");

            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.ViewDate)
                .HasColumnType("datetime")
                .HasColumnName("viewDate");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlListingViewHistories)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingViewHistory_TL_ListingRent");

            entity.HasOne(d => d.User).WithMany(p => p.TlListingViewHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_ListingViewHistory_TU_User");
        });

        modelBuilder.Entity<TlQuickTypeView>(entity =>
        {
            entity.HasKey(e => e.QuickTipViewId);

            entity.ToTable("TL_QuickTypeView");

            entity.Property(e => e.QuickTipViewId).HasColumnName("quickTipViewId");
            entity.Property(e => e.QuickTipId).HasColumnName("quickTipId");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ViewTypeId).HasColumnName("viewTypeId");

            entity.HasOne(d => d.QuickTip).WithMany(p => p.TlQuickTypeViews)
                .HasForeignKey(d => d.QuickTipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_QuickTypeView_T_QuickTip");

            entity.HasOne(d => d.ViewType).WithMany(p => p.TlQuickTypeViews)
                .HasForeignKey(d => d.ViewTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TL_QuickTypeView_TL_ViewType");
        });

        modelBuilder.Entity<TlStayPresence>(entity =>
        {
            entity.HasKey(e => e.StayPresenceId).HasName("PK__TL_StayP__E0A2D2872098B4DC");

            entity.ToTable("TL_StayPresence");

            entity.Property(e => e.StayPresenceId).HasColumnName("stayPresenceId");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.StayPrecenseTypeId).HasColumnName("stayPrecenseTypeId");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TlStayPresences)
                .HasForeignKey(d => d.ListingRentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TL_StayPr__listi__2C1E8537");

            entity.HasOne(d => d.StayPrecenseType).WithMany(p => p.TlStayPresences)
                .HasForeignKey(d => d.StayPrecenseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TL_StayPr__stayP__56B3DD81");
        });

        modelBuilder.Entity<TlStepsType>(entity =>
        {
            entity.HasKey(e => e.StepsTypeId);

            entity.ToTable("TL_StepsType");

            entity.Property(e => e.StepsTypeId).HasColumnName("stepsTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CodeParent)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codeParent");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TlViewType>(entity =>
        {
            entity.HasKey(e => e.ViewTypeId);

            entity.ToTable("TL_ViewType");

            entity.Property(e => e.ViewTypeId).HasColumnName("viewTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NextViewTypeId).HasColumnName("nextViewTypeId");
            entity.Property(e => e.StepTypeId).HasColumnName("stepTypeId");
            entity.Property(e => e.ViewTypeIdParent).HasColumnName("viewTypeIdParent");

            entity.HasOne(d => d.StepType).WithMany(p => p.TlViewTypes)
                .HasForeignKey(d => d.StepTypeId)
                .HasConstraintName("FK_TL_ViewType_TL_StepsType");
        });

        modelBuilder.Entity<TmConversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId);

            entity.ToTable("TM_Conversation");

            entity.Property(e => e.ConversationId).HasColumnName("conversationId");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.UserIdOne).HasColumnName("userId_one");
            entity.Property(e => e.UserIdTwo).HasColumnName("userId_two");

            entity.HasOne(d => d.Status).WithMany(p => p.TmConversations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TM_Conversation_TM_ConversationStatus");

            entity.HasOne(d => d.UserIdOneNavigation).WithMany(p => p.TmConversationUserIdOneNavigations)
                .HasForeignKey(d => d.UserIdOne)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TM_Conversation_TU_User");

            entity.HasOne(d => d.UserIdTwoNavigation).WithMany(p => p.TmConversationUserIdTwoNavigations)
                .HasForeignKey(d => d.UserIdTwo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TM_Conversation_TU_User1");
        });

        modelBuilder.Entity<TmConversationStatus>(entity =>
        {
            entity.HasKey(e => e.ConversationStatusId);

            entity.ToTable("TM_ConversationStatus");

            entity.Property(e => e.ConversationStatusId).HasColumnName("conversationStatusId");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TmMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId);

            entity.ToTable("TM_Message");

            entity.Property(e => e.MessageId).HasColumnName("messageId");
            entity.Property(e => e.AdditionalData)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("additionalData");
            entity.Property(e => e.Body)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("body");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.ConversationId).HasColumnName("conversationID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DateExecution).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ipAddress");
            entity.Property(e => e.IsRead).HasColumnName("isRead");
            entity.Property(e => e.MessageStatusId).HasColumnName("messageStatusId");
            entity.Property(e => e.ReadDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Conversation).WithMany(p => p.TmMessages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TM_Message_TM_Conversation");

            entity.HasOne(d => d.MessageStatus).WithMany(p => p.TmMessages)
                .HasForeignKey(d => d.MessageStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TM_Message_TM_MessageStatus");

            entity.HasOne(d => d.MessageType).WithMany(p => p.TmMessages)
                .HasForeignKey(d => d.MessageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TM_Message_TM_TypeMessage");
        });

        modelBuilder.Entity<TmMessageStatus>(entity =>
        {
            entity.HasKey(e => e.MessageStatus);

            entity.ToTable("TM_MessageStatus");

            entity.Property(e => e.MessageStatus).HasColumnName("messageStatus");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TmNotification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__TM_Notif__20CF2E32DA1D3DBF");

            entity.ToTable("TM_Notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.MessageNotification).HasColumnType("text");
            entity.Property(e => e.NotificationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferecceTable)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");
            entity.Property(e => e.SendChannel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SendStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SubjectNotification)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TmNotifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TM_Notifi__UserI__5E54FF49");
        });

        modelBuilder.Entity<TmTypeMessage>(entity =>
        {
            entity.HasKey(e => e.TypeMessageId);

            entity.ToTable("TM_TypeMessage");

            entity.Property(e => e.TypeMessageId).HasColumnName("typeMessageId");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TpAmenitiesCategory>(entity =>
        {
            entity.HasKey(e => e.AmenitiesCategoryId);

            entity.ToTable("TP_AmenitiesCategory");

            entity.Property(e => e.AmenitiesCategoryId).HasColumnName("amenitiesCategoryId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TpAmenitiesType>(entity =>
        {
            entity.HasKey(e => e.AmenitiesTypeId);

            entity.ToTable("TP_AmenitiesType");

            entity.Property(e => e.AmenitiesTypeId).HasColumnName("amenitiesTypeId");
            entity.Property(e => e.AmenitiesCategoryId).HasColumnName("amenitiesCategoryId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IconLink)
                .IsUnicode(false)
                .HasColumnName("iconLink");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.AmenitiesCategory).WithMany(p => p.TpAmenitiesTypes)
                .HasForeignKey(d => d.AmenitiesCategoryId)
                .HasConstraintName("FK_TP_AmenitiesType_TP_AmenitiesCategory");
        });

        modelBuilder.Entity<TpEstimatedRentalIncome>(entity =>
        {
            entity.HasKey(e => e.EstimatedRentalIncomeId);

            entity.ToTable("TP_EstimatedRentalIncome");

            entity.Property(e => e.EstimatedRentalIncomeId).HasColumnName("estimatedRentalIncomeId");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.IncomeFrom)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("incomeFrom");
            entity.Property(e => e.IncomeTo)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("incomeTo");
            entity.Property(e => e.PropertySubtypeId).HasColumnName("propertySubtypeId");
            entity.Property(e => e.TimeFrom).HasColumnName("timeFrom");
            entity.Property(e => e.TimeTo).HasColumnName("timeTo");
            entity.Property(e => e.UnitId).HasColumnName("unitId");

            entity.HasOne(d => d.Currency).WithMany(p => p.TpEstimatedRentalIncomes)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_TP_EstimatedRentalIncome_T_Currency");

            entity.HasOne(d => d.PropertySubtype).WithMany(p => p.TpEstimatedRentalIncomes)
                .HasForeignKey(d => d.PropertySubtypeId)
                .HasConstraintName("FK_TP_EstimatedRentalIncome_TP_PropertySubtype");

            entity.HasOne(d => d.Unit).WithMany(p => p.TpEstimatedRentalIncomes)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK_TP_EstimatedRentalIncome_T_Unit");
        });

        modelBuilder.Entity<TpProperty>(entity =>
        {
            entity.HasKey(e => e.PropertyId);

            entity.ToTable("TP_Property");

            entity.Property(e => e.PropertyId).HasColumnName("propertyId");
            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address2");
            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CountyId).HasColumnName("countyId");
            entity.Property(e => e.CountyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ExternalReference)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("externalReference");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.PropertySubtypeId).HasColumnName("propertySubtypeId");
            entity.Property(e => e.StateId).HasColumnName("stateId");
            entity.Property(e => e.StateName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("zipCode");

            entity.HasOne(d => d.City).WithMany(p => p.TpProperties)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_TP_Property_T_City");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TpProperties)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TP_Property_TL_ListingRent");

            entity.HasOne(d => d.PropertySubtype).WithMany(p => p.TpProperties)
                .HasForeignKey(d => d.PropertySubtypeId)
                .HasConstraintName("FK_TP_Property_TP_PropertySubtype");

            entity.HasOne(d => d.State).WithMany(p => p.TpProperties)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_TP_Property_T_State");
        });

        modelBuilder.Entity<TpPropertyAddress>(entity =>
        {
            entity.HasKey(e => e.PropertyAddressId);

            entity.ToTable("TP_PropertyAddress");

            entity.Property(e => e.PropertyAddressId).HasColumnName("propertyAddressId");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("address2");
            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.CountyId).HasColumnName("countyId");
            entity.Property(e => e.PropertyId).HasColumnName("propertyId");
            entity.Property(e => e.StateId).HasColumnName("stateId");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("zipCode");

            entity.HasOne(d => d.City).WithMany(p => p.TpPropertyAddresses)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_TP_PropertyAddress_T_City");

            entity.HasOne(d => d.County).WithMany(p => p.TpPropertyAddresses)
                .HasForeignKey(d => d.CountyId)
                .HasConstraintName("FK_TP_PropertyAddress_T_County");

            entity.HasOne(d => d.Property).WithMany(p => p.TpPropertyAddresses)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK_TP_PropertyAddress_TP_Property");

            entity.HasOne(d => d.State).WithMany(p => p.TpPropertyAddresses)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_TP_PropertyAddress_T_State");
        });

        modelBuilder.Entity<TpPropertySubtype>(entity =>
        {
            entity.HasKey(e => e.PropertySubtypeId);

            entity.ToTable("TP_PropertySubtype");

            entity.Property(e => e.PropertySubtypeId).HasColumnName("propertySubtypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Icon)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PropertyDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("propertyDescription");
            entity.Property(e => e.PropertyTypeId).HasColumnName("propertyTypeId");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.PropertyType).WithMany(p => p.TpPropertySubtypes)
                .HasForeignKey(d => d.PropertyTypeId)
                .HasConstraintName("FK_TP_PropertySubtype_TP_PropertyType");
        });

        modelBuilder.Entity<TpPropertyType>(entity =>
        {
            entity.HasKey(e => e.PropertyTypeId);

            entity.ToTable("TP_PropertyType");

            entity.Property(e => e.PropertyTypeId).HasColumnName("propertyTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TpRuleType>(entity =>
        {
            entity.HasKey(e => e.RuleTypeId);

            entity.ToTable("TP_RuleType");

            entity.Property(e => e.RuleTypeId).HasColumnName("ruleTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IconLink)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("iconLink");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TpSecurityItemType>(entity =>
        {
            entity.HasKey(e => e.SecurityItemTypeId);

            entity.ToTable("TP_SecurityItemType");

            entity.Property(e => e.SecurityItemTypeId).HasColumnName("securityItemTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IconLink)
                .IsUnicode(false)
                .HasColumnName("iconLink");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TsInsurance>(entity =>
        {
            entity.HasKey(e => e.InsuranceId).HasName("PK__TS_Insur__79D82ED085DFB192");

            entity.ToTable("TS_Insurances");

            entity.Property(e => e.InsuranceId).HasColumnName("insuranceId");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CoverageTypeId).HasColumnName("coverageTypeId");
            entity.Property(e => e.InsuranceDescription)
                .HasColumnType("text")
                .HasColumnName("insuranceDescription");
            entity.Property(e => e.InsuranceName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("insuranceName");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<TuAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("TU_Account");

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.DateLastLogin)
                .HasColumnType("datetime")
                .HasColumnName("dateLastLogin");
            entity.Property(e => e.ForceChange).HasColumnName("forceChange");
            entity.Property(e => e.IncorrectAccess).HasColumnName("incorrectAccess");
            entity.Property(e => e.IpLastLogin)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ipLastLogin");
            entity.Property(e => e.IsBlocked).HasColumnName("isBlocked");
            entity.Property(e => e.LastBlockDate)
                .HasColumnType("datetime")
                .HasColumnName("lastBlockDate");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TemporaryBlockTo)
                .HasColumnType("datetime")
                .HasColumnName("temporaryBlockTo");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.TuAccounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_Account_TU_User");
        });

        modelBuilder.Entity<TuAccountType>(entity =>
        {
            entity.HasKey(e => e.AccountTypeId).HasName("PK_AccountType");

            entity.ToTable("TU_AccountType");

            entity.Property(e => e.AccountTypeId).HasColumnName("accountTypeId");
            entity.Property(e => e.AccountType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("accountType");
            entity.Property(e => e.AccountTypeCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("accountTypeCode");
        });

        modelBuilder.Entity<TuAdditionalProfile>(entity =>
        {
            entity.HasKey(e => e.AdditionalProfileId);

            entity.ToTable("TU_AdditionalProfile");

            entity.Property(e => e.AdditionalProfileId).HasColumnName("additionalProfileId");
            entity.Property(e => e.Additional)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("additional");
            entity.Property(e => e.Pets)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("pets");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.WantedToGo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("wantedToGo");
            entity.Property(e => e.WhatIdo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("whatIdo");

            entity.HasOne(d => d.User).WithMany(p => p.TuAdditionalProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_AdditionalProfile_TU_User");
        });

        modelBuilder.Entity<TuAdditionalProfileLanguage>(entity =>
        {
            entity.HasKey(e => e.AdditionalProfileLanguageId).HasName("PK_TU_AddtionalProfileLanguage");

            entity.ToTable("TU_AdditionalProfileLanguage");

            entity.Property(e => e.AdditionalProfileLanguageId).HasColumnName("additionalProfileLanguageId");
            entity.Property(e => e.AdditionalProfileId).HasColumnName("additionalProfileId");
            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.AdditionalProfile).WithMany(p => p.TuAdditionalProfileLanguages)
                .HasForeignKey(d => d.AdditionalProfileId)
                .HasConstraintName("FK_TU_AddtionalProfileLanguage_TU_AdditionalProfile");

            entity.HasOne(d => d.Language).WithMany(p => p.TuAdditionalProfileLanguages)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_TU_AddtionalProfileLanguage_T_Language");
        });

        modelBuilder.Entity<TuAdditionalProfileLiveAt>(entity =>
        {
            entity.HasKey(e => e.AdditionalProfileLiveAtId);

            entity.ToTable("TU_AdditionalProfileLiveAt");

            entity.Property(e => e.AdditionalProfileLiveAtId).HasColumnName("additionalProfileLiveAtId");
            entity.Property(e => e.AdditionalProfileId).HasColumnName("additionalProfileId");
            entity.Property(e => e.CityName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("cityName");
            entity.Property(e => e.Location)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.StateId).HasColumnName("stateId");

            entity.HasOne(d => d.AdditionalProfile).WithMany(p => p.TuAdditionalProfileLiveAts)
                .HasForeignKey(d => d.AdditionalProfileId)
                .HasConstraintName("FK_TU_AdditionalProfileLiveAt_TU_AdditionalProfile");

            entity.HasOne(d => d.State).WithMany(p => p.TuAdditionalProfileLiveAts)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_TU_AdditionalProfileLiveAt_T_State");
        });

        modelBuilder.Entity<TuAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId);

            entity.ToTable("TU_Address");

            entity.Property(e => e.AddressId).HasColumnName("addressId");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("address2");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.IsCurrent).HasColumnName("isCurrent");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.StateId).HasColumnName("stateId");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("zipCode");

            entity.HasOne(d => d.State).WithMany(p => p.TuAddresses)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_TU_Address_T_State");

            entity.HasOne(d => d.User).WithMany(p => p.TuAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_Address_TU_User");
        });

        modelBuilder.Entity<TuCard>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TU_Card");

            entity.Property(e => e.CardId).HasColumnName("cardId");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cardNumber");
            entity.Property(e => e.CardToken)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("cardToken");
            entity.Property(e => e.IsPrincipal).HasColumnName("isPrincipal");
            entity.Property(e => e.ProcesorCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("procesorCode");
            entity.Property(e => e.ProcesorName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("procesorName");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_Card_TU_User");
        });

        modelBuilder.Entity<TuDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId);

            entity.ToTable("TU_Document");

            entity.Property(e => e.DocumentId).HasColumnName("documentId");
            entity.Property(e => e.Approved).HasColumnName("approved");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("documentNumber");
            entity.Property(e => e.DocumentTypeId).HasColumnName("documentTypeId");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("fileName");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.TuDocuments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_Document_TU_User");
        });

        modelBuilder.Entity<TuEmail>(entity =>
        {
            entity.HasKey(e => e.EmailId);

            entity.ToTable("TU_Email");

            entity.Property(e => e.EmailId).HasColumnName("emailId");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsPrincipal).HasColumnName("isPrincipal");
            entity.Property(e => e.IsRecover).HasColumnName("isRecover");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.TuEmails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_Email_TU_User");
        });

        modelBuilder.Entity<TuEmergencyContact>(entity =>
        {
            entity.HasKey(e => e.EmergencyContactId).HasName("PK__TU_Emerg__7394A15D5FD195B2");

            entity.ToTable("TU_EmergencyContact");

            entity.Property(e => e.EmergencyContactId).HasColumnName("emergencyContactId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.LstName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("lstName");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhoneCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phoneCode");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Relationship)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("relationship");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Language).WithMany(p => p.TuEmergencyContacts)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_EmergencyContact_Language");

            entity.HasOne(d => d.User).WithMany(p => p.TuEmergencyContacts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_EmergencyContact_TU_User");
        });

        modelBuilder.Entity<TuGenderType>(entity =>
        {
            entity.HasKey(e => e.GenderTypeId);

            entity.ToTable("TU_GenderType");

            entity.Property(e => e.GenderTypeId).HasColumnName("genderTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TuPhone>(entity =>
        {
            entity.HasKey(e => e.PhoneId);

            entity.ToTable("TU_Phone");

            entity.Property(e => e.PhoneId).HasColumnName("phoneId");
            entity.Property(e => e.AreaCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("areaCode");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("countryCode");
            entity.Property(e => e.IsMobile).HasColumnName("isMobile");
            entity.Property(e => e.IsPrimary).HasColumnName("isPrimary");
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("number");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.TuPhones)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_Phone_TU_User");
        });

        modelBuilder.Entity<TuPlatform>(entity =>
        {
            entity.HasKey(e => e.PlatformId);

            entity.ToTable("TU_Platform");

            entity.Property(e => e.PlatformId).HasColumnName("platformId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TuProfilePhoto>(entity =>
        {
            entity.HasKey(e => e.ProfilePhotoId);

            entity.ToTable("TU_ProfilePhoto");

            entity.Property(e => e.ProfilePhotoId).HasColumnName("profilePhotoId");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("fileName");
            entity.Property(e => e.IsPrincipal).HasColumnName("isPrincipal");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.TuProfilePhotos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_ProfilePhoto_TU_User");
        });

        modelBuilder.Entity<TuTitleType>(entity =>
        {
            entity.HasKey(e => e.TitleTypeId);

            entity.ToTable("TU_TitleType");

            entity.Property(e => e.TitleTypeId).HasColumnName("titleTypeId");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("abbreviation");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TuUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("TU_User");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.AccountType).HasColumnName("accountType");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.FavoriteName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("favoriteName");
            entity.Property(e => e.GenderTypeId).HasColumnName("genderTypeId");
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhotoLink)
                .IsUnicode(false)
                .HasColumnName("photoLink");
            entity.Property(e => e.PlatformId).HasColumnName("platformId");
            entity.Property(e => e.RegisterDate)
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
            entity.Property(e => e.SocialId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("socialId");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TimeZoneId).HasColumnName("timeZoneId");
            entity.Property(e => e.TitleTypeId).HasColumnName("titleTypeId");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.UserStatusTypeId).HasColumnName("userStatusTypeId");

            entity.HasOne(d => d.AccountTypeNavigation).WithMany(p => p.TuUsers)
                .HasForeignKey(d => d.AccountType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_User_TU_AccountType");

            entity.HasOne(d => d.GenderType).WithMany(p => p.TuUsers)
                .HasForeignKey(d => d.GenderTypeId)
                .HasConstraintName("FK_TU_User_TU_GenderType");

            entity.HasOne(d => d.Platform).WithMany(p => p.TuUsers)
                .HasForeignKey(d => d.PlatformId)
                .HasConstraintName("FK_TU_User_TU_Platform");

            entity.HasOne(d => d.TitleType).WithMany(p => p.TuUsers)
                .HasForeignKey(d => d.TitleTypeId)
                .HasConstraintName("FK_TU_User_TU_TitleType");

            entity.HasOne(d => d.UserStatusType).WithMany(p => p.TuUsers)
                .HasForeignKey(d => d.UserStatusTypeId)
                .HasConstraintName("FK_TU_User_TU_UserStatusType");
        });

        modelBuilder.Entity<TuUserListingRent>(entity =>
        {
            entity.HasKey(e => e.UserListingRentId);

            entity.ToTable("TU_UserListingRent");

            entity.Property(e => e.UserListingRentId).HasColumnName("userListingRentId");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.DateTimeTransaction)
                .HasColumnType("datetime")
                .HasColumnName("dateTimeTransaction");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserListingRentStatusId).HasColumnName("userListingRentStatusId");
            entity.Property(e => e.UserListingStatusId).HasColumnName("userListingStatusId");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TuUserListingRents)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TU_UserListingRent_TL_ListingRent");

            entity.HasOne(d => d.User).WithMany(p => p.TuUserListingRents)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_UserListingRent_TU_User");

            entity.HasOne(d => d.UserListingStatus).WithMany(p => p.TuUserListingRents)
                .HasForeignKey(d => d.UserListingStatusId)
                .HasConstraintName("FK_TU_UserListingRent_TU_UserListingStatus");
        });

        modelBuilder.Entity<TuUserListingRentPayment>(entity =>
        {
            entity.HasKey(e => e.UserListingRentPaymentId);

            entity.ToTable("TU_UserListingRentPayment");

            entity.Property(e => e.UserListingRentPaymentId).HasColumnName("userListingRentPaymentId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UserListingRentId).HasColumnName("userListingRentId");

            entity.HasOne(d => d.Currency).WithMany(p => p.TuUserListingRentPayments)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_TU_UserListingRentPayment_T_Currency");

            entity.HasOne(d => d.UserListingRent).WithMany(p => p.TuUserListingRentPayments)
                .HasForeignKey(d => d.UserListingRentId)
                .HasConstraintName("FK_TU_UserListingRentPayment_TU_UserListingRent1");
        });

        modelBuilder.Entity<TuUserListingStatus>(entity =>
        {
            entity.HasKey(e => e.UserListingStatusId);

            entity.ToTable("TU_UserListingStatus");

            entity.Property(e => e.UserListingStatusId).HasColumnName("userListingStatusId");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TuUserLog>(entity =>
        {
            entity.HasKey(e => e.UserLogId);

            entity.ToTable("TU_UserLog");

            entity.Property(e => e.UserLogId).HasColumnName("userLogID");
            entity.Property(e => e.ApplicationCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("applicationCode");
            entity.Property(e => e.BrowseInfo)
                .HasMaxLength(3000)
                .IsUnicode(false)
                .HasColumnName("browseInfo");
            entity.Property(e => e.ColumnName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("columnName");
            entity.Property(e => e.Command)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("command");
            entity.Property(e => e.DateRegister)
                .HasColumnType("datetime")
                .HasColumnName("dateRegister");
            entity.Property(e => e.Ip)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ip");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ipAddress");
            entity.Property(e => e.IsMobile).HasColumnName("isMobile");
            entity.Property(e => e.JsonData)
                .IsUnicode(false)
                .HasColumnName("jsonData");
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tableName");
            entity.Property(e => e.TypeLog)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("typeLog");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserRegId).HasColumnName("userRegId");

            entity.HasOne(d => d.User).WithMany(p => p.TuUserLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TU_UserLog_TU_User");
        });

        modelBuilder.Entity<TuUserReview>(entity =>
        {
            entity.HasKey(e => e.UserReviewId);

            entity.ToTable("TU_UserReview");

            entity.Property(e => e.UserReviewId).HasColumnName("userReviewId");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Calification).HasColumnName("calification");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.DateTimeReview)
                .HasColumnType("datetime")
                .HasColumnName("dateTimeReview");
            entity.Property(e => e.ListingRentId).HasColumnName("listingRentId");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserIdReviewer).HasColumnName("userId_reviewer");

            entity.HasOne(d => d.Book).WithMany(p => p.TuUserReviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_TU_UserReview_TB_Book");

            entity.HasOne(d => d.ListingRent).WithMany(p => p.TuUserReviews)
                .HasForeignKey(d => d.ListingRentId)
                .HasConstraintName("FK_TU_UserReview_TL_ListingRent");

            entity.HasOne(d => d.User).WithMany(p => p.TuUserReviewUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_UserReview_TU_User");

            entity.HasOne(d => d.UserIdReviewerNavigation).WithMany(p => p.TuUserReviewUserIdReviewerNavigations)
                .HasForeignKey(d => d.UserIdReviewer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_UserReview_TU_User1");
        });

        modelBuilder.Entity<TuUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId);

            entity.ToTable("TU_UserRole");

            entity.Property(e => e.UserRoleId).HasColumnName("userRoleId");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserTypeId).HasColumnName("userTypeId");

            entity.HasOne(d => d.User).WithMany(p => p.TuUserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_UserRole_TU_User");

            entity.HasOne(d => d.UserType).WithMany(p => p.TuUserRoles)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TU_UserRole_TU_UserType");
        });

        modelBuilder.Entity<TuUserStatusType>(entity =>
        {
            entity.HasKey(e => e.UserStatusTypeId);

            entity.ToTable("TU_UserStatusType");

            entity.Property(e => e.UserStatusTypeId).HasColumnName("userStatusTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TuUserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId);

            entity.ToTable("TU_UserType");

            entity.Property(e => e.UserTypeId).HasColumnName("userTypeId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
