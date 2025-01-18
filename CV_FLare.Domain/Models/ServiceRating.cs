using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CV_FLare.Domain.Models;

public class ServiceRating
{
    [Key]
    public int RatingId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    public int ServiceId { get; set; }

    [ForeignKey("ServiceId")]
    public virtual Package Service { get; set; } = null!;

    public int? RatingIcon { get; set; }

    public DateTime? CreatedAt { get; set; }
}
