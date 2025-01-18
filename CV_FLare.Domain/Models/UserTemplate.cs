using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_FLare.Domain.Models;

public class UserTemplate
{
    [Key]
    public int UserTemplatesId { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public int TemplateId { get; set; }

    [ForeignKey("TemplateId")]
    public virtual Template Template { get; set; } = null!;

    [Required]
    public string CvName { get; set; } = null!;

    [Required]
    public string TemplateContent { get; set; } = null!;

    public int JobDescId { get; set; }

    [ForeignKey("JobDescId")]
    public virtual JobDescription JobDesc { get; set; } = null!;

    public int AiScore { get; set; }

    public bool IsDraft { get; set; }

    public int? TemplateProgress { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }
}
