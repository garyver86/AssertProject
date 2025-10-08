using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUser
{
    public int UserId { get; set; }

    public int? PlatformId { get; set; }

    public string? UserName { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public int? GenderTypeId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? TitleTypeId { get; set; }

    public int? UserStatusTypeId { get; set; }

    public DateTime? RegisterDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? PhotoLink { get; set; }

    public string? Status { get; set; }

    public int AccountType { get; set; }

    public string? SocialId { get; set; }

    public int? TimeZoneId { get; set; }

    public string? FavoriteName { get; set; }

    public bool? BlockAsHost { get; set; }

    public DateTime? BlockAsHostDateTime { get; set; }

    public virtual TuAccountType AccountTypeNavigation { get; set; } = null!;

    public virtual TuGenderType? GenderType { get; set; }

    public virtual ICollection<PayPriceCalculation> PayPriceCalculations { get; set; } = new List<PayPriceCalculation>();

    public virtual TuPlatform? Platform { get; set; }

    public virtual ICollection<TExceptionLog> TExceptionLogs { get; set; } = new List<TExceptionLog>();

    public virtual ICollection<TbBook> TbBookCancellationUsers { get; set; } = new List<TbBook>();

    public virtual ICollection<TbBookCancellation> TbBookCancellations { get; set; } = new List<TbBookCancellation>();

    public virtual ICollection<TbBookInsuranceClaim> TbBookInsuranceClaims { get; set; } = new List<TbBookInsuranceClaim>();

    public virtual ICollection<TbBook> TbBookUserIdRenterNavigations { get; set; } = new List<TbBook>();

    public virtual ICollection<TbComplaint> TbComplaintComplainantUsers { get; set; } = new List<TbComplaint>();

    public virtual ICollection<TbComplaint> TbComplaintReportedHosts { get; set; } = new List<TbComplaint>();

    public virtual ICollection<TiIssue> TiIssueRelatedUsers { get; set; } = new List<TiIssue>();

    public virtual ICollection<TiIssue> TiIssueReportedByUsers { get; set; } = new List<TiIssue>();

    public virtual TuTitleType? TitleType { get; set; }

    public virtual ICollection<TlGeneralAdditionalFee> TlGeneralAdditionalFees { get; set; } = new List<TlGeneralAdditionalFee>();

    public virtual ICollection<TlListingFavoriteGroup> TlListingFavoriteGroups { get; set; } = new List<TlListingFavoriteGroup>();

    public virtual ICollection<TlListingFavorite> TlListingFavorites { get; set; } = new List<TlListingFavorite>();

    public virtual ICollection<TlListingRent> TlListingRents { get; set; } = new List<TlListingRent>();

    public virtual ICollection<TlListingReview> TlListingReviews { get; set; } = new List<TlListingReview>();

    public virtual ICollection<TlListingViewHistory> TlListingViewHistories { get; set; } = new List<TlListingViewHistory>();

    public virtual ICollection<TmConversation> TmConversationUserIdOneNavigations { get; set; } = new List<TmConversation>();

    public virtual ICollection<TmConversation> TmConversationUserIdTwoNavigations { get; set; } = new List<TmConversation>();

    public virtual ICollection<TmNotification> TmNotifications { get; set; } = new List<TmNotification>();

    public virtual ICollection<TnNotification> TnNotifications { get; set; } = new List<TnNotification>();

    public virtual ICollection<TuAccount> TuAccounts { get; set; } = new List<TuAccount>();

    public virtual ICollection<TuAdditionalProfile> TuAdditionalProfiles { get; set; } = new List<TuAdditionalProfile>();

    public virtual ICollection<TuAddress> TuAddresses { get; set; } = new List<TuAddress>();

    public virtual ICollection<TuDocument> TuDocuments { get; set; } = new List<TuDocument>();

    public virtual ICollection<TuEmail> TuEmails { get; set; } = new List<TuEmail>();

    public virtual ICollection<TuEmergencyContact> TuEmergencyContacts { get; set; } = new List<TuEmergencyContact>();

    public virtual ICollection<TuOwnerConfiguration> TuOwnerConfigurations { get; set; } = new List<TuOwnerConfiguration>();

    public virtual ICollection<TuPhone> TuPhones { get; set; } = new List<TuPhone>();

    public virtual ICollection<TuProfilePhoto> TuProfilePhotos { get; set; } = new List<TuProfilePhoto>();

    public virtual ICollection<TuUserListingRent> TuUserListingRents { get; set; } = new List<TuUserListingRent>();

    public virtual ICollection<TuUserLog> TuUserLogs { get; set; } = new List<TuUserLog>();

    public virtual ICollection<TuUserOtp> TuUserOtps { get; set; } = new List<TuUserOtp>();

    public virtual ICollection<TuUserReview> TuUserReviewUserIdReviewerNavigations { get; set; } = new List<TuUserReview>();

    public virtual ICollection<TuUserReview> TuUserReviewUsers { get; set; } = new List<TuUserReview>();

    public virtual ICollection<TuUserRole> TuUserRoles { get; set; } = new List<TuUserRole>();

    public virtual TuUserStatusType? UserStatusType { get; set; }
}
