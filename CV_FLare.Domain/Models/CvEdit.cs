using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class CvEdit
{
    public int EditId { get; set; }

    public int SubmissionId { get; set; }

    public int? EditedBy { get; set; }

    public string? EditedContent { get; set; }

    public string? Feedback { get; set; }

    public DateTime? EditedAt { get; set; }

    public virtual User? EditedByNavigation { get; set; }

    public virtual CvSubmission Submission { get; set; } = null!;
}
