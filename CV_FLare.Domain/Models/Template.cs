using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class Template
{
    public int TemplateId { get; set; }

    public string? TemplateName { get; set; }

    public string? PreviewImg { get; set; }

    public string? TemplateFile { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<UserTemplate> UserTemplates { get; set; } = new List<UserTemplate>();
}
