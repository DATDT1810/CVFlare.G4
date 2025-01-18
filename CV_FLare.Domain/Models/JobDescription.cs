using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CV_FLare.Domain.Models;

public class JobDescription
{
    [Key]
    public int JobDescId { get; set; }
    [Required]
    [MaxLength(255)]
    public string JobTitle { get; set; }
    [Required, MaxLength(255)]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<CvSubmission> CvSubmissions { get; set; } = new List<CvSubmission>();

    public virtual ICollection<UserTemplate> UserTemplates { get; set; } = new List<UserTemplate>();
}
