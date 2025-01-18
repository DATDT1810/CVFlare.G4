using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_FLare.Domain.Models;

public class Package
{
    [Key]
    public int PackageId { get; set; }

    [Required]
    [MaxLength(100)]
    public string PackageName { get; set; }
    [Required]
    public string PackageDescription { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PackagePrice { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<CvSubmission> CvSubmissions { get; set; } = new List<CvSubmission>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ServiceRating> ServiceRatings { get; set; } = new List<ServiceRating>();
}
