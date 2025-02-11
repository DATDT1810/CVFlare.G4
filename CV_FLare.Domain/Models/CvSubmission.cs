using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_FLare.Domain.Models;

public partial class CvSubmission
{
    [Key]
    public int SubmissionId { get; set; }

    public int PackageId { get; set; }
    [ForeignKey("PackageId")]
    public virtual Package Package { get; set; } = null!;

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public string? FilePath { get; set; }

    public string JobDescripion { get; set; }

    public double? AiScore { get; set; }

    public DateTime? DueDate { get; set; }

    [Required]
    public string Status { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual ICollection<CvEdit> CvEdits { get; set; } = new List<CvEdit>();

}
