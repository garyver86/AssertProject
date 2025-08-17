using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TReviewQuestion
{
    public int ReviewQuestionId { get; set; }

    public string? QuestionTitle { get; set; }

    public string? QuestionText { get; set; }

    public string QuestionCode { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<TlListingReviewQuestion> TlListingReviewQuestions { get; set; } = new List<TlListingReviewQuestion>();
}
