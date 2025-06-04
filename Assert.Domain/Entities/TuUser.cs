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

    public virtual TuAccountType AccountTypeNavigation { get; set; } = null!;

    public virtual TuGenderType? GenderType { get; set; }

    public virtual TuPlatform? Platform { get; set; }

    public virtual ICollection<TExceptionLog> TExceptionLogs { get; set; } = new List<TExceptionLog>();

    public virtual ICollection<TbBookInsuranceClaim> TbBookInsuranceClaims { get; set; } = new List<TbBookInsuranceClaim>();

    public virtual ICollection<TiIssue> TiIssueRelatedUsers { get; set; } = new List<TiIssue>();

    public virtual ICollection<TiIssue> TiIssueReportedByUsers { get; set; } = new List<TiIssue>();

    public virtual TuTitleType? TitleType { get; set; }

    public virtual ICollection<TlListingFavorite> TlListingFavorites { get; set; } = new List<TlListingFavorite>();

    public virtual ICollection<TlListingRent> TlListingRents { get; set; } = new List<TlListingRent>();

    public virtual ICollection<TlListingReview> TlListingReviews { get; set; } = new List<TlListingReview>();

    public virtual ICollection<TmConversation> TmConversationUserIdOneNavigations { get; set; } = new List<TmConversation>();

    public virtual ICollection<TmConversation> TmConversationUserIdTwoNavigations { get; set; } = new List<TmConversation>();

    public virtual ICollection<TmNotification> TmNotifications { get; set; } = new List<TmNotification>();

    public virtual ICollection<TuAccount> TuAccounts { get; set; } = new List<TuAccount>();

    public virtual ICollection<TuAddress> TuAddresses { get; set; } = new List<TuAddress>();

    public virtual ICollection<TuDocument> TuDocuments { get; set; } = new List<TuDocument>();

    public virtual ICollection<TuEmail> TuEmails { get; set; } = new List<TuEmail>();

    public virtual ICollection<TuPhone> TuPhones { get; set; } = new List<TuPhone>();

    public virtual ICollection<TuProfilePhoto> TuProfilePhotos { get; set; } = new List<TuProfilePhoto>();

    public virtual ICollection<TuUserListingRent> TuUserListingRents { get; set; } = new List<TuUserListingRent>();

    public virtual ICollection<TuUserLog> TuUserLogs { get; set; } = new List<TuUserLog>();

    public virtual ICollection<TuUserReview> TuUserReviewUserIdReviewerNavigations { get; set; } = new List<TuUserReview>();

    public virtual ICollection<TuUserReview> TuUserReviewUsers { get; set; } = new List<TuUserReview>();

    public virtual ICollection<TuUserRole> TuUserRoles { get; set; } = new List<TuUserRole>();

    public virtual TuUserStatusType? UserStatusType { get; set; }
}
