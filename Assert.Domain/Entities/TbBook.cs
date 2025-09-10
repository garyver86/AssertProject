using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBook
{
    public long BookId { get; set; }

    public long ListingRentId { get; set; }

    public int UserIdRenter { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal AmountTotal { get; set; }

    public int CurrencyId { get; set; }

    public decimal? MountPerNight { get; set; }

    public decimal? AmountFees { get; set; }

    public string NameRenter { get; set; } = null!;

    public string LastNameRenter { get; set; } = null!;

    public bool? TermsAccepted { get; set; }

    public string? AdditionalInfo { get; set; }

    public int BookStatusId { get; set; }

    public bool? IsApprobal { get; set; }

    public string? ApprovalDetails { get; set; }

    public bool? IsManualApprobal { get; set; }

    public int? DaysToApproval { get; set; }

    public DateTime? InitDate { get; set; }

    public string? PaymentCode { get; set; }

    public string? Pk { get; set; }

    public string? PaymentId { get; set; }

    public decimal? DepositSec { get; set; }

    public string? PickUpLocation { get; set; }

    public decimal? VggFee { get; set; }

    public decimal? VggFeePercent { get; set; }

    public DateTime? Checkin { get; set; }

    public DateTime? Checkout { get; set; }

    public DateTime? MaxCheckin { get; set; }

    public DateTime? GuestCheckin { get; set; }

    public DateTime? GuestCheckout { get; set; }

    public DateTime? CancellationStart { get; set; }

    public DateTime? CancellationEnd { get; set; }

    public DateTime? Cancellation { get; set; }

    public int? CancellationUserId { get; set; }

    public virtual TbBookStatus BookStatus { get; set; } = null!;

    public virtual TCurrency Currency { get; set; } = null!;

    public virtual TlListingRent ListingRent { get; set; } = null!;

    public virtual ICollection<PayPriceCalculation> PayPriceCalculations { get; set; } = new List<PayPriceCalculation>();

    public virtual ICollection<TbBookChange> TbBookChanges { get; set; } = new List<TbBookChange>();

    public virtual ICollection<TbBookInsuranceClaim> TbBookInsuranceClaims { get; set; } = new List<TbBookInsuranceClaim>();

    public virtual ICollection<TbBookPayment> TbBookPayments { get; set; } = new List<TbBookPayment>();

    public virtual ICollection<TbBookSnapshot> TbBookSnapshots { get; set; } = new List<TbBookSnapshot>();

    public virtual ICollection<TbBookStep> TbBookSteps { get; set; } = new List<TbBookStep>();

    public virtual ICollection<TbBookingInsurance> TbBookingInsurances { get; set; } = new List<TbBookingInsurance>();

    public virtual ICollection<TiIssue> TiIssues { get; set; } = new List<TiIssue>();

    public virtual ICollection<TlListingAvailability> TlListingAvailabilities { get; set; } = new List<TlListingAvailability>();

    public virtual ICollection<TlListingCalendar> TlListingCalendars { get; set; } = new List<TlListingCalendar>();

    public virtual ICollection<TlListingReview> TlListingReviews { get; set; } = new List<TlListingReview>();

    public virtual ICollection<TmConversation> TmConversations { get; set; } = new List<TmConversation>();

    public virtual ICollection<TnNotification> TnNotifications { get; set; } = new List<TnNotification>();

    public virtual ICollection<TuUserReview> TuUserReviews { get; set; } = new List<TuUserReview>();

    public virtual TuUser UserIdRenterNavigation { get; set; } = null!;
}
