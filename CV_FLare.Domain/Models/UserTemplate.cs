using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class UserTemplate
{
    public int UserTemplatesId { get; set; }

    public int? UserId { get; set; }

    public int? TemplateId { get; set; }

    public string? CvName { get; set; }

    public string? TemplateContent { get; set; }

    public int? JobDescId { get; set; }

    public int? AiScore { get; set; }

    public bool? IsDraft { get; set; }

    public int? TemplateProgress { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual JobDescription? JobDesc { get; set; }

    public virtual Template? Template { get; set; }

    public virtual User? User { get; set; }
}
