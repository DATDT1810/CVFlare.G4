using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CV_FLare.Domain.Models;

public class CvEdit
{
    [Key]
    public int EditId { get; set; }

    public int SubmissionId { get; set; }

    [ForeignKey("SubmissionId")]
    public virtual CvSubmission Submission { get; set; } = null!;

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User? EditedByNavigation { get; set; }

    public string? EditedContent { get; set; }
    [MaxLength(1000)]
    public string? Feedback { get; set; }

    public DateTime EditedAt { get; set; }
}
