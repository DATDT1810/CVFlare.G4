using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class Package
{
    public int PackageId { get; set; }

    public string? PackageName { get; set; }

    public string? PackageDescription { get; set; }

    public decimal? PackagePrice { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<CvSubmission> CvSubmissions { get; set; } = new List<CvSubmission>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ServiceRating> ServiceRatings { get; set; } = new List<ServiceRating>();
}
