using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class JobDescription
{
    public int JobDescId { get; set; }

    public string? JobTitle { get; set; }

    public string? JobDescription1 { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<CvSubmission> CvSubmissions { get; set; } = new List<CvSubmission>();

    public virtual ICollection<UserTemplate> UserTemplates { get; set; } = new List<UserTemplate>();
}
