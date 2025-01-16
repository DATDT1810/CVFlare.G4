using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class CvSubmission
{
    public int SubmissionId { get; set; }

    public int PackageId { get; set; }

    public int? UserId { get; set; }

    public string? FilePath { get; set; }

    public int? JobDescId { get; set; }

    public double? AiScore { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Status { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual ICollection<CvEdit> CvEdits { get; set; } = new List<CvEdit>();

    public virtual JobDescription? JobDesc { get; set; }

    public virtual Package Package { get; set; } = null!;

    public virtual User? User { get; set; }
}
